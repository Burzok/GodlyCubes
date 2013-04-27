using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PlayerData {
		public NetworkViewID id;
		public string name;
		public Vector3 color;
		public int kills;
		public int deaths;
		public int assist;	
};

public class PlayerList : MonoBehaviour {
	
	public List<PlayerData> playerList = new List<PlayerData>(4);

	[RPC] //Server function
	void RegisterPlayer(string playerName, NetworkViewID playerID) {
		
		PlayerData player = new PlayerData();
		
		player.id = playerID;
		player.name = playerName;
		
		if (playerList.Count == 0)
			player.color = new Vector3(1,0,0);
		else
			player.color = new Vector3(0,0,1);

		player.kills=0;
		player.deaths=0;
		player.assist=0;		
		playerList.Add(player);
		networkView.RPC("UpdatePlayer", RPCMode.AllBuffered, player.id, player.color);
		networkView.RPC("InfoToClient", RPCMode.OthersBuffered, player.id, player.name, player.color, player.kills, player.deaths, player.assist);
	}
	
	[RPC] //Server function
	void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);		
	}
	
	[RPC] //Server & Client function
	void UpdatePlayer(NetworkViewID id, Vector3 color) {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
			if (id == player.networkView.viewID) {					
				player.GetComponentInChildren<Renderer>().material.color = new Color(color[0],color[1],color[2]);
			}
		}
	}
	
	[RPC] //Server function
	void GetPlayerInfo(NetworkViewID idToFind) { 
		
		PlayerData playerToSend = new PlayerData();
		playerToSend = playerList.Find(playerToFind => playerToFind.id == idToFind);
		
		networkView.RPC("InfoToClient",idToFind.owner,idToFind.owner ,playerToSend.name, playerToSend.color, playerToSend.kills, playerToSend.deaths, playerToSend.assist);
	}
	
	[RPC] //Client function
	void InfoToClient(NetworkViewID id, string playerName, Vector3 color, int kills, int deaths, int assist)	{
		
		PlayerData player = new PlayerData();
		
		player.id=id;
		player.name=playerName;
		player.color=color;
		player.kills=kills;
		player.deaths=deaths;
		player.assist=assist;
		
		playerList.Add(player);
	}
	
	void OnGUI() {
		if (Network.isClient || Network.isServer) { 
			GUI.Box (new Rect(5f, 50f, 140f, 50f),"");
			GUI.BeginGroup(new Rect(5f, 50f, 140f, 50f));
			foreach(PlayerData player in playerList) {
				GUILayout.BeginHorizontal();
				
	        	if (player.color == new Vector3(1, 0, 0))  
	          		GUI.contentColor = Color.red;
	       		if (player.color == new Vector3(0, 0, 1))
	          		GUI.contentColor = Color.blue;
	       
				GUILayout.Space(5f);				
				GUILayout.Label(player.name+": ");
				
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
}
