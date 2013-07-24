using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void DrawGUI();

public enum Map { 
	CrystalCaverns,
	BurzokGrounds
}

public class MenuGUI : MonoBehaviour {
	public DrawGUI drawGUI;
	public bool drawChat;
	public bool drawStats;
	
	public Map selectedMap;
	
	private Networking networking;
	private Rotator myPlayerRotator;
	private Rect windowRect = new Rect(Screen.width * .5f-200f, 20f, 400f, 150f);
	private float timeHostWasRegistered;
	public GameObject miniCrystalCaverns, miniBurzokGrounds;
	private List<PlayerData> playerList;
	
	private int lastLevelPrefix;
	private string level;
	
	void Awake() {
		lastLevelPrefix = 0;
		level = "CrystalCaverns";
		
		networking = GetComponent<Networking>();
		playerList = GetComponent<PlayerList>().playerList;
		
		drawGUI = DrawMainMenu;
		selectedMap = Map.CrystalCaverns;
	}
	
	void OnGUI() {
		drawGUI();
	}
	
	private void DrawMainMenu() {
		if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Create Server"))
			SetCreateServerState();

		if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Connect")) 
			SetConnectState();
		
		if (GUI.Button(new Rect(Screen.width * 0.5f-50f, Screen.height * 0.5f, 100f, 50f), "Options")) 
			SetOptionsMenuState();

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Quit"))
			Application.Quit();
	}

	private void DrawCreateServer() {
		Rect screenCoordinates = new Rect(Screen.width*0.5f-50f, 30f, 100f, 20f);
		networking.SetServerName(GUI.TextField(screenCoordinates, networking.GetServerName() ));

		if (GUI.Button (new Rect(Screen.width*0.5f-50f, 55f, 100f, 50f), "Create")) {
  			Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  		MasterServer.RegisterHost("GodlyCubesLight", networking.GetServerName() );
			timeHostWasRegistered = Time.time;
			SetServerGameState();
			drawChat = true;
			drawStats = true;
			
			networkView.RPC("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix+1);
		}

		if (GUI.Button (new Rect(Screen.width*0.5f-50f, Screen.height - 70f, 100f, 50f),"Back")) {
			miniBurzokGrounds.SetActive(false);
			miniCrystalCaverns.SetActive(false);
			SetMainMenuState();
		}
		
		GUI.Box(new Rect(Screen.width*0.05f, Screen.height*0.2f, Screen.width*0.3f, Screen.height*0.2f), "Select Map");
		if (GUI.Button (new Rect(Screen.width*0.07f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.1f),"Crystal Caverns")) {
			selectedMap = Map.CrystalCaverns;
			miniCrystalCaverns.SetActive(true);
			miniBurzokGrounds.SetActive(false);
		}
		if (GUI.Button (new Rect(Screen.width*0.23f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.1f),"Burzok Grounds")) {
			selectedMap = Map.BurzokGrounds;
			miniBurzokGrounds.SetActive(true);
			miniCrystalCaverns.SetActive(false);
		}
		
		GUI.Box(new Rect(Screen.width*0.05f ,Screen.height*0.6f, Screen.width*0.3f, Screen.height*0.2f), "Select Mode");
		if (GUI.Button (new Rect(Screen.width*0.07f, Screen.height*0.65f, Screen.width*0.1f, Screen.height*0.1f),"Classic"));
			//TODO: Funkcja do wyboru trybu classic
		if (GUI.Button (new Rect(Screen.width*0.23f, Screen.height*0.65f, Screen.width*0.1f, Screen.height*0.1f),"Weird"));
			//TODO: Funkcja do wyboru trybu weird
	}
	
	[RPC]
	private void LoadLevel(string level, int levelPrefix, NetworkMessageInfo info) {
		lastLevelPrefix = levelPrefix;

		Network.SetLevelPrefix(levelPrefix);
		
		if(Network.isServer)
			Application.LoadLevel(level+"Server");
		else
			Application.LoadLevel(level+"Client");
	}
	
	void OnLevelWasLoaded(int level) {
		
		if(level == 3) {
	        foreach (NetworkPlayer player in Network.connections) {
	            Network.SetReceivingEnabled(player, 0, true);
	        }
			Network.SetSendingEnabled(0, true);
		}
			
		if(level == 1) {
			miniCrystalCaverns = GameObject.Find("miniCrystalCaverns");
			miniBurzokGrounds = GameObject.Find("miniBurzokGrounds");
			
			miniCrystalCaverns.SetActive(false);
			miniBurzokGrounds.SetActive(false);
		}
    }
	
	void OnDisconnectedFromServer () {
		Application.LoadLevel(networking.disconnectedLevel);
	}

	private void DrawConnect() {
		windowRect = GUI.Window(0, windowRect, ServerList, "Server List");
		
		Rect screenCoordinates = new Rect(Screen.width * .5f-50f, 175f, 100f, 20f);
		networking.SetPlayerName(GUI.TextField(screenCoordinates, networking.GetPlayerName()));

		if (GUI.Button (new Rect(Screen.width*0.5f-50f, Screen.height-70f, 100f, 50f), "Back"))
			SetMainMenuState();
	}
	
	private void ServerList(int windowID) {
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
				networking.SetServerName(element.gameName);
				SetConnectingState();
			}

			GUILayout.EndHorizontal();	
		}
	}
	
	private void DrawOptionsMenu() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 180f), "TODO: Make options");
		
		if (GUI.Button(new Rect(Screen.width*0.5f-50f,Screen.height*0.7f,100f,30f), "Back"))
			SetMainMenuState();			
	}
	
	private void DrawTeamSelect() {
		if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Team A")) {
			networking.ConnectToGame(Team.TeamA);
			SetClientGameState();
			drawChat = true;
			drawStats = true;
		}

		if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Team B")) {
			networking.ConnectToGame(Team.TeamB);
			SetClientGameState();
			drawChat = true;
			drawStats = true;
		}

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Disconnect")) {			
			Network.Disconnect();
			SetMainMenuState();
		}
	}

	private void DrawServerGame() {
		GUI.Label(new Rect(5,5,250,40), "Server name: " + networking.GetServerName());

		if (GUI.Button (new Rect(Screen.width-105f, 5f, 100f, 50f), "Back")) {				 
			if(Network.isServer)
   				networkView.RPC("ExitCL", RPCMode.Others);

			GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
			foreach(GameObject player in players) {
				Network.Destroy(player.networkView.viewID);	
			}

			Network.Disconnect();
			Application.LoadLevel(0);
		}
	}

	private void DrawClientGame() {
		GUI.Label(new Rect(5,5,250,40), "Server name: " + networking.GetServerName());

		if (GUI.Button(new Rect(Screen.width-105f, 5f, 100f, 50f), "Back")) {  				
			networkView.RPC("UnregisterPlayer", RPCMode.Server, networking.getMyPlayerID());
			networkView.RPC("DecCounters", RPCMode.AllBuffered, (int)networking.actualTeam);
			Network.Disconnect();
			Application.LoadLevel(0);
		}
		
		DrawStats();
	}
	
	private void DrawStats() {
		if (drawStats) { 
			GUI.Box (new Rect(5f, 50f, 140f, 50f),"");
			GUI.BeginGroup(new Rect(5f, 50f, 140f, 50f));
			foreach(PlayerData player in playerList) {
				GUILayout.BeginHorizontal();
				
	        	if (player.color == new Vector3(1, 0, 0))  
	          		GUI.contentColor = Color.red;
	       		if (player.color == new Vector3(0, 0, 1))
	          		GUI.contentColor = Color.blue;
	       
				GUILayout.Space(5f);
				GUILayout.Label(player.playerName+": ");
				
				GUI.contentColor = Color.green;
				GUILayout.Label("K: "+player.kills);
				GUILayout.FlexibleSpace();
				
				GUI.contentColor = Color.red;
				GUILayout.Label("D: "+player.deaths);
				GUILayout.FlexibleSpace();
				
				GUI.contentColor = Color.yellow;
				GUILayout.Label("A: "+player.assist);
				GUILayout.FlexibleSpace();
				
				GUILayout.EndHorizontal();
			}
			GUI.EndGroup();
		}
	}
	
	private void DrawWinState() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 80f), "You Win");
		Input.ResetInputAxes();
		
		if (GUI.Button(new Rect(Screen.width*0.5f-50f,Screen.height*0.7f,100f,30f), "Disconnect")) {
			Network.Disconnect();
			Application.LoadLevel(0);
		}
	}
	
	private void DrawLoseState() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 80f), "You Lose");
		Input.ResetInputAxes();
		
		
		if (GUI.Button(new Rect(Screen.width*0.5f-50f,Screen.height*0.7f,100f,30f), "Disconnect")) {
			Network.Disconnect();
			Application.LoadLevel(0);
		}
	}
	
	private void DrawConnecting() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 180f), "CONNECTING ...");
	}
	
	 
	
	public void SetWinState() {
		drawChat = false;
		drawStats = false;		
		if (Network.isClient) {
			myPlayerRotator = GetComponent<PlayerList>().myPlayer.GetComponent<Rotator>();	
			myPlayerRotator.enabled = false;
			Screen.lockCursor = false;
		}
		drawGUI = DrawWinState;
	}
	
	public void SetLoseState() {
		drawChat = false;
		drawStats = false;
		Screen.lockCursor = false;
		if (Network.isClient) {
			myPlayerRotator = GetComponent<PlayerList>().myPlayer.GetComponent<Rotator>();	
			myPlayerRotator.enabled = false;
			Screen.lockCursor = false;
		}
		drawGUI = DrawLoseState;
	}
	
	public void SetMainMenuState() {
		drawGUI = DrawMainMenu;
	}
	
	public void SetTeamSelectState() {
		drawGUI = DrawTeamSelect;
	}
	
	public void SetServerGameState() {
		drawGUI = DrawServerGame;
	}
	
	public void SetClientGameState() {
		drawGUI = DrawClientGame;
	}
	
	public void SetCreateServerState() {
		drawGUI = DrawCreateServer;
	}
	
	public void SetConnectState() {
		drawGUI = DrawConnect;
	}
	
	public void SetOptionsMenuState() {
		drawGUI = DrawOptionsMenu;
	}
	
	public void SetConnectingState() {
		drawGUI = DrawConnecting;
	}
}
