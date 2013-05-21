using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {
	public NetworkViewID id;
	public Transform lokalTransform;
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
		
		GameObject[] lokalPlayers = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach (GameObject lokalPlayer in lokalPlayers) {
			if (player.id == lokalPlayer.networkView.viewID)
				player.lokalTransform = lokalPlayer.transform;
		}
		
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
		SynchronizeClient(playerID);
	}
	
	private void SynchronizeClient(NetworkViewID clientID) {
		foreach(PlayerData data in playerList) {
			networkView.RPC("SynchronizeList", clientID.owner, data.id, data.kills, data.deaths, data.assist);
		}
	}
	
	[RPC]
	private void SynchronizeList(NetworkViewID id, int kills, int deaths, int assist) {
		int ind = playerList.FindIndex(player => player.id == id);
		playerList[ind].kills=kills;
		playerList[ind].deaths=deaths;
		playerList[ind].assist=assist;
	}
	
	[RPC] //Server function
	void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);		
	}
	
	[RPC] //Server & Client function
	void UpdatePlayer(NetworkViewID id, Vector3 color) {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		ChangeColor(ref id, ref color, ref players);
		
		ChangePosition(ref id, ref players);
	}
	
	private void ChangeColor(ref NetworkViewID id, ref Vector3 color, ref GameObject[] players) {
		
		foreach(GameObject player in players) {
			if (id == player.networkView.viewID) {					
				player.GetComponentInChildren<Renderer>().material.color = new Color(color[0],color[1],color[2]);
			}
		}
	}
	
	private void ChangePosition(ref NetworkViewID id, ref GameObject[] players) {
		int numberOfPlayers = playerList.Count;
		
		Transform spawningPoint = gameObject.GetComponent<Networking>().FindSpawn(ref numberOfPlayers);
		foreach(GameObject player in players) {
			if (id == player.networkView.viewID) {
				player.GetComponent<PlayerGameData>().respawnPosition = spawningPoint.position;
				player.transform.position = spawningPoint.position;
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
