using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct PlayerData
{
		public NetworkViewID id;
		public string name;
		public Vector3 color;
		public int kills;
		public int deaths;
		public int assist;	
};

public class PlayerList : MonoBehaviour {
	
	List<PlayerData> playerList = new List<PlayerData>(2);

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
	}
	
	[RPC] //Server function
	void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);		
	}
	
	[RPC] //Server & Client function
	void UpdatePlayer(NetworkViewID id, Vector3 color) {
		GameObject []players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players)
		{
			if (id == player.networkView.viewID)
			{					
				player.GetComponentInChildren<Renderer>().material.color = new Color(color[0],color[1],color[2]);
			}
		}
	}
	
	[RPC] //Server function
	void GetPlayerInfo(NetworkViewID idToFind){ 
		
		PlayerData playerToSend = new PlayerData();
		playerToSend = playerList.Find(playerToFind => playerToFind.id == idToFind);
		
		string playername = playerToSend.name;
		if (playerToSend.color == new Vector3(1, 0, 0))	
			playername=playername+": ";
			//playername="<color=red>"+playername+": ";
		
		if (playerToSend.color == new Vector3(0, 0, 1))
			playername=playername+": ";
			//playername="<color=blue>"+playername+": ";
		
		networkView.RPC("InfoToClient",idToFind.owner,playername);
	}
}
