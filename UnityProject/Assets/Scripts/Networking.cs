using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	
	enum MenuState{Main, CreateServer, Connect, ServerGame, ClientGame};
	
	string serverName = "Server Name";
	float timeHostWasRegistered;	
	Rect windowRect = new Rect(Screen.width * .5f-150f, 20f, 300f, 150f);
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
		}
		
 		if(guiState==MenuState.CreateServer) { 
			serverName = GUI.TextField(new Rect(Screen.width * .5f-50f, 20f, 100f, 20f), serverName);
			if (GUI.Button (new Rect(Screen.width * .5f-50f, 50f, 100f, 50f),"Create")) {
  				Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  			MasterServer.RegisterHost("GodlyCubesLight", serverName);
				timeHostWasRegistered = Time.time;
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
			
			if (GUI.Button (new Rect(Screen.width * .5f-50f, Screen.height - 70f, 100f, 50f),"Back"))
				guiState = MenuState.Main;
		}			
	}
	
	void OnConnectedToServer() {	 
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);		
	}
	
	[RPC]
	void ExitCL() {
  		Application.Quit();
	}
	
	void ServerList (int windowID) {
		if(Time.time - timeHostWasRegistered >= 1.0f)
			{
				MasterServer.RequestHostList("GodlyCubesLight");
			}
			HostData[] data = MasterServer.PollHostList();
		// Go through all the hosts in the host list
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
					// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
					Network.Connect(element);			
				}
				GUILayout.EndHorizontal();	
			}
	}
}