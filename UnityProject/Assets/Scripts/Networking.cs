using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	public GameObject player_prefab;
	
	enum MenuState{Main, CreateServer, Connect, ServerGame, ClientGame};
	string serverName = "Server Name";
	string playerName = "Player Name";
	float timeHostWasRegistered;	
	Rect windowRect = new Rect(Screen.width * .5f-200f, 20f, 400f, 150f);
	MenuState guiState=MenuState.Main;
	
	void Awake () {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");	
	}
	
	void OnGUI () {		
		if (guiState==MenuState.Main) {
			if (GUI.Button (new Rect(Screen.width * .5f-120f, 20f, 100f, 50f),"Create Server"))
				guiState = MenuState.CreateServer;
				
			if (GUI.Button (new Rect(Screen.width * .5f+20f, 20f, 100f, 50f),"Connect")) 
				guiState = MenuState.Connect;
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Quit"))
				Application.Quit();
		}
		
 		if(guiState==MenuState.CreateServer) { 
			serverName = GUI.TextField(new Rect(Screen.width * .5f-50f, 5f, 100f, 20f), serverName);
			playerName = GUI.TextField(new Rect(Screen.width * .5f-50f, 30f, 100f, 20f), playerName);
			if (GUI.Button (new Rect(Screen.width * .5f-50f, 55f, 100f, 50f),"Create")) {
  				Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  			MasterServer.RegisterHost("GodlyCubesLight", serverName);
				timeHostWasRegistered = Time.time;
				GameObject spawner = GameObject.Find ("Spawner");
				Network.Instantiate(player_prefab, spawner.transform.position, spawner.transform.rotation, 0);
		 		foreach (GameObject go in FindObjectsOfType(typeof(GameObject))) {
		 			 go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);	
				}
				guiState=MenuState.ServerGame;
			}
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
				guiState = MenuState.Main;
		}	
			
		if(guiState==MenuState.Connect) {  
			windowRect = GUI.Window (0, windowRect, ServerList, "Server List");
			playerName = GUI.TextField(new Rect(Screen.width * .5f-50f, 175f, 100f, 20f), playerName);
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
				guiState = MenuState.Main;
		}
		
		if(guiState==MenuState.ServerGame) {  
			if(Network.isServer) {
				GUI.Label(new Rect(10,10,250,40),"Server name: " + serverName);
  			}    
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back")) {				 
				if(Network.isServer)
   					networkView.RPC("ExitCL", RPCMode.Others);
   				
				Network.Disconnect();
				guiState = MenuState.Main;
			}
		}
		
		if(guiState==MenuState.ClientGame) {
			GUI.Label(new Rect(10,10,250,40),"Server name: " + serverName);
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back")) {  				
				Network.Disconnect();
				guiState = MenuState.Main;
			}
		}
	}	
	
	void OnConnectedToServer() {			
		GameObject spawner = GameObject.Find ("Spawner");
		Network.Instantiate(player_prefab, spawner.transform.position, spawner.transform.rotation, 0);
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);		
	}
	
	[RPC]
	void ExitCL() {
  		Network.Disconnect();
		guiState = MenuState.Main;
	}
	
	void ServerList (int windowID) {
		if(Time.time - timeHostWasRegistered >= 1.0f)
			{
				MasterServer.RequestHostList("GodlyCubesLight");
			}
			HostData[] data = MasterServer.PollHostList();
			
		foreach (var element in data)
		{
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
			if (GUILayout.Button("Connect"))
			{
				Network.Connect(element);
				serverName = element.gameName;
				guiState = MenuState.ClientGame;					
			}
			GUILayout.EndHorizontal();	
		}
	}
}