using UnityEngine;
using System.Collections;

public delegate void DrawGUI();

public enum Team {
	TeamA,
	TeamB	
}

public class Networking : MonoBehaviour {
	
	public GameObject player_prefab;
	public DrawGUI drawGUI;

	private string serverName = "Server Name";
	private string playerName = "Player Name";
	private GameObject goPlayer;
	private float timeHostWasRegistered;	
	private Rect windowRect = new Rect(Screen.width * .5f-200f, 20f, 400f, 150f);
	private GameObject spawners;
	private Transform tempTransform;
	Team actualTeam;
	private int numOfPlayersA = 0;
	private int numOfPlayersB = 0;
	
	void Awake () {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");

		spawners = GameObject.Find("Spawners");

		drawGUI = DrawMainMenu;
	}

	void OnGUI() {
		drawGUI();
	}

	private void DrawMainMenu() {
		if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Create Server"))
			drawGUI = DrawCreateServer;

		if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Connect")) 
			drawGUI = DrawConnect;

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Quit"))
			Application.Quit();
	}

	private void DrawCreateServer() {
		serverName = GUI.TextField(new Rect(Screen.width * 0.5f-50f, 30f, 100f, 20f), serverName);

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, 55f, 100f, 50f),"Create")) {
  			Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  		MasterServer.RegisterHost("GodlyCubesLight", serverName);
			timeHostWasRegistered = Time.time;
			GameObject spawner = GameObject.Find("Spawner");
			drawGUI = DrawServerGame;
		}

		if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
			drawGUI = DrawMainMenu;
	}	

	private void DrawConnect() {
		windowRect = GUI.Window(0, windowRect, ServerList, "Server List");
		playerName = GUI.TextField(new Rect(Screen.width * .5f-50f, 175f, 100f, 20f), playerName);

		if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
			drawGUI = DrawMainMenu;
	}
	
	private void DrawTeamSelect() {
		if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Team A")) {
			drawGUI = DrawClientGame;
			ConnectToGame(Team.TeamA);
		}

		if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Team B")) {
			drawGUI = DrawClientGame;
			ConnectToGame(Team.TeamB);
		}

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Disconnect")) {			
			Network.Disconnect();
			drawGUI = DrawMainMenu;
		}
	}

	private void DrawServerGame() {
		GUI.Label(new Rect(5,5,250,40),"Server name: " + serverName);

		if (GUI.Button (new Rect(Screen.width-105f, 5f, 100f, 50f),"Back")) {				 
			if(Network.isServer)
   				networkView.RPC("ExitCL", RPCMode.Others);

			GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
			foreach(GameObject player in players) {
				Network.Destroy(player.networkView.viewID);					
			}

			Network.Disconnect();
			drawGUI = DrawMainMenu;
		}
	}

	private void DrawClientGame() {
		GUI.Label(new Rect(5,5,250,40), "Server name: " + serverName);

			if (GUI.Button (new Rect(Screen.width-105f, 5f, 100f, 50f), "Back")) {  				
				networkView.RPC("UnregisterPlayer", RPCMode.Server, goPlayer.networkView.viewID);			
				GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);

				Network.Disconnect();
				drawGUI = DrawMainMenu;
			}
	}
	
	void OnConnectedToServer() {
		drawGUI = DrawTeamSelect;	
	}
	
	private void ConnectToGame(Team playerTeam) {
		networkView.RPC("DownloadTeamCounterData", RPCMode.Server, networkView.owner);
		networkView.RPC("IncCounters", RPCMode.AllBuffered, (int)playerTeam);
		Transform spawner = FindSpawn(playerTeam);
		goPlayer = SpawnPlayer(ref spawner);
		networkView.RPC("RegisterPlayer", RPCMode.Server, playerName, goPlayer.networkView.viewID, (int)playerTeam);
	}
	
	[RPC]
	private void DownloadTeamCounterData(NetworkPlayer playerID) {
		networkView.RPC("SendTeamCounterData", playerID, numOfPlayersA, numOfPlayersB);
	}
	
	[RPC]
	private void SendTeamCounterData(int TeamA, int TeamB) {
		numOfPlayersA = TeamA;
		numOfPlayersB = TeamB;
	}
	
	[RPC]
	private void IncCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TeamA)
			numOfPlayersA++;
		else if ((Team)playerTeam == Team.TeamB)
			numOfPlayersB++;
	}
	
	private GameObject SpawnPlayer(ref Transform spawner) {
		return Network.Instantiate(player_prefab, spawner.transform.position, spawner.transform.rotation, 0) 
			as GameObject;
	}
	
	public Transform FindSpawn(Team playerTeam) {
		if (playerTeam == Team.TeamA) {
			Transform TA = spawners.transform.FindChild("TeamA");
			return TA.FindChild("Spawn"+numOfPlayersA);
		}
		else if (playerTeam == Team.TeamB) {
			Transform TB = spawners.transform.FindChild("TeamB");
			return TB.FindChild("Spawn"+numOfPlayersB);
		}
		else {
			Debug.Log("Error wrong spawn select");
			return spawners.transform;
		}
	}
	
/*
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
*/
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
		drawGUI = DrawMainMenu;
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
			}

			GUILayout.EndHorizontal();	
		}
	}

	public NetworkViewID getPlayerID() {
		return goPlayer.networkView.viewID;
	}
}