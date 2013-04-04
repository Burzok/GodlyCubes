using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	
	string IPServer = "127.0.0.1";
	string IPServ = "";
	float timeHostWasRegistered;
	
	void Awake () {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");	
	}
	
	void OnGUI () {		
		if (Network.peerType == NetworkPeerType.Disconnected) {
			IPServer = GUI.TextField(new Rect(120,10,100,20),IPServer);
			
			if (GUI.Button (new Rect(10,10,100,30),"Conect"))
		 		Network.Connect(IPServer, 25000);	
			
			if (GUI.Button (new Rect(10,50,100,30),"Start Server")) {
				Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
	  			MasterServer.RegisterHost("GodlyCubesLight", "Godly Cubes Testserver", "Testing GodlyCubes" );
				timeHostWasRegistered = Time.time;
		 		foreach (GameObject go in FindObjectsOfType(typeof(GameObject))) {
		 			 go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);	
				}
			}
	 	} 
 		else{  
  			if(Network.isServer) {
				IPServ = Network.player.ipAddress;
				GUI.Label(new Rect(140,20,250,40),"IP to be used: "+IPServ );
  			}    
			
  			if (GUI.Button (new Rect(10,10,100,30),"Exit")){
				
   				if(Network.isServer)
   					networkView.RPC("ExitCL", RPCMode.Others);
   				
				Network.Disconnect();
  				Application.Quit();
  			}
 		}
		
		if (Time.time - timeHostWasRegistered >= 1.0f)
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
				hostInfo = hostInfo + host + ":" + element.port + " ";
			hostInfo = hostInfo + "]";
			GUILayout.Label(hostInfo);	
			GUILayout.Space(5);
			GUILayout.Label(element.comment);
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
	
	void OnConnectedToServer() {	 
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnLoaded", SendMessageOptions.DontRequireReceiver);		
	}
	
	[RPC]
	void ExitCL() {
  		Application.Quit();
	}
}