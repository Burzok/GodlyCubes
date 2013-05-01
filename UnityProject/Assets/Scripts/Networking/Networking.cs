using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	public GameObject player_prefab;
	// TODO: Marcin jesli to sa zmienne prywatne to je nazwij jako prywatne
	enum MenuState{Main, CreateServer, Connect, ServerGame, ClientGame};
	MenuState guiState=MenuState.Main;
	string serverName = "Server Name";
	string playerName = "Player Name";
	GameObject goPlayer;
	float timeHostWasRegistered;	
	Rect windowRect = new Rect(Screen.width * .5f-200f, 20f, 400f, 150f);
	private GameObject spawners;
	
	void Awake () {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");
		
		spawners = GameObject.Find("Spawners");
	}
	
	void OnGUI () {
		if (guiState==MenuState.Main) {
			if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Create Server"))
				guiState = MenuState.CreateServer;
				
			if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Connect")) 
				guiState = MenuState.Connect;
			
			if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Quit"))
				Application.Quit();
		}
		
 		if(guiState==MenuState.CreateServer) { 
			serverName = GUI.TextField(new Rect(Screen.width * 0.5f-50f, 30f, 100f, 20f), serverName);
			
			if (GUI.Button (new Rect(Screen.width * 0.5f-50f, 55f, 100f, 50f),"Create")) {
  				Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  			MasterServer.RegisterHost("GodlyCubesLight", serverName);
				timeHostWasRegistered = Time.time;
				GameObject spawner = GameObject.Find("Spawner");
				guiState = MenuState.ServerGame;
			}
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
				guiState = MenuState.Main;
		}	
			
		if(guiState==MenuState.Connect) {  
			windowRect = GUI.Window(0, windowRect, ServerList, "Server List");
			playerName = GUI.TextField(new Rect(Screen.width * .5f-50f, 175f, 100f, 20f), playerName);
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
				guiState = MenuState.Main;
		}
		
		if(guiState==MenuState.ServerGame) {  
				GUI.Label(new Rect(5,5,250,40),"Server name: " + serverName);
			
			if (GUI.Button (new Rect(Screen.width-105f, 5f, 100f, 50f),"Back")) {				 
				if(Network.isServer)
   					networkView.RPC("ExitCL", RPCMode.Others);
				
				GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
				foreach(GameObject player in players) {
					Network.Destroy(player.networkView.viewID);					
				}
				
				Network.Disconnect();
				guiState = MenuState.Main;
			}
		}
		
		if(guiState==MenuState.ClientGame) {
			GUI.Label(new Rect(5,5,250,40), "Server name: " + serverName);
			
			if (GUI.Button (new Rect(Screen.width-105f, 5f, 100f, 50f), "Back")) {  				
				networkView.RPC("UnregisterPlayer", RPCMode.Server, goPlayer.networkView.viewID);
				GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);

				Network.Disconnect();
				guiState = MenuState.Main;
			}
		}
	}	
	
	void OnConnectedToServer() {			
		Transform spawner = spawners.transform;
		goPlayer = SpawnPlayer(ref spawner);
		networkView.RPC("RegisterPlayer", RPCMode.Server, playerName, goPlayer.networkView.viewID);
	}
	
	public Transform FindSpawn(ref int numberOfPlayers) {
		if (numberOfPlayers == 0) 
			return spawners.transform.FindChild("Spawn1");
		else if (numberOfPlayers == 1)
			return spawners.transform.FindChild("Spawn2");
		else if (numberOfPlayers == 2)
			return spawners.transform.FindChild("Spawn3");
		else if (numberOfPlayers == 3)
			return spawners.transform.FindChild("Spawn4");
		else
			return spawners.transform;
	}
	
	private GameObject SpawnPlayer(ref Transform spawner) {
		return Network.Instantiate(player_prefab, spawner.transform.position, spawner.transform.rotation, 0) 
			as GameObject;
	}
	
	void OnDisconnectedFromServer () {
		GameObject []players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			GameObject.Destroy(player);					
		}
		GetComponent<PlayerList>().playerList.Clear();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	[RPC]
	void ExitCL() {
  		Network.Disconnect();
		guiState = MenuState.Main;
	}
	
	void ServerList (int windowID) {
		if(Time.time - timeHostWasRegistered >= 1.0f) {
			MasterServer.RequestHostList("GodlyCubesLight");
		}
		
		HostData[] data = MasterServer.PollHostList();
		foreach (HostData element in data) {
			GUILayout.BeginHorizontal();	
			var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
			GUILayout.Label(name);	
			GUILayout.Space(5);
			
			string hostInfo;
			hostInfo = "[";
			foreach (var host in element.ip)
				hostInfo = hostInfo + host;
			hostInfo = hostInfo + "]";
			
			GUILayout.Label(hostInfo);	
			GUILayout.Space(5);
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("Connect")) {
				Network.Connect(element);
				serverName = element.gameName;
				guiState = MenuState.ClientGame;					
			}
			
			GUILayout.EndHorizontal();	
		}
	}
	
	public NetworkViewID getPlayerID() {
		return goPlayer.networkView.viewID;
	}
}