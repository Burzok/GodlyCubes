using UnityEngine;
using System.Collections;

public delegate void DrawGUI();

public class MenuGUI : MonoBehaviour {
	public DrawGUI drawGUI;
	public bool drawChat;
	public bool drawStats;
	
	private Networking networking;
	private Rect windowRect = new Rect(Screen.width * .5f-200f, 20f, 400f, 150f);
	private float timeHostWasRegistered;
	
	void Awake() {
		networking = GetComponent<Networking>();
		
		drawGUI = DrawMainMenu;
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
		}

		if (GUI.Button (new Rect(Screen.width*0.5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
			SetMainMenuState();
	}	

	private void DrawConnect() {
		windowRect = GUI.Window(0, windowRect, ServerList, "Server List");
		
		Rect screenCoordinates = new Rect(Screen.width * .5f-50f, 175f, 100f, 20f);
		networking.SetPlayerName(GUI.TextField(screenCoordinates, networking.GetPlayerName()));

		if (GUI.Button (new Rect(Screen.width*0.5f-50f, Screen.height-70f, 100f, 50f), "Back"))
			SetMainMenuState();
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
			SetMainMenuState();
		}
	}

	private void DrawClientGame() {
		GUI.Label(new Rect(5,5,250,40), "Server name: " + networking.GetServerName());

		if (GUI.Button(new Rect(Screen.width-105f, 5f, 100f, 50f), "Back")) {  				
			networkView.RPC("UnregisterPlayer", RPCMode.Server, networking.getPlayerID());
			networkView.RPC("DecCounters", RPCMode.AllBuffered, (int)networking.actualTeam);
			Network.Disconnect();
			SetMainMenuState();
		}
	}
	
	private void DrawWinState() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 80f), "You Win");
		Input.ResetInputAxes();
		
		if (GUI.Button(new Rect(Screen.width*0.5f-50f,Screen.height*0.7f,100f,30f), "Disconnect")) {
			Network.Disconnect();
			SetMainMenuState();
		}
	}
	
	private void DrawLoseState() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 80f), "You Lose");
		Input.ResetInputAxes();
		
		
		if (GUI.Button(new Rect(Screen.width*0.5f-50f,Screen.height*0.7f,100f,30f), "Disconnect")) {
			Network.Disconnect();
			SetMainMenuState();
		}
	}
	
	private void DrawConnecting() {
		GUI.Box(new Rect(Screen.width*0.5f-100f, 50f, 200f, 180f), "CONNECTING ...");
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
	
	public void SetWinState() {
		drawChat = false;
		drawStats = false;
		drawGUI = DrawWinState;
	}
	
	public void SetLoseState() {
		drawChat = false;
		drawStats = false;
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
