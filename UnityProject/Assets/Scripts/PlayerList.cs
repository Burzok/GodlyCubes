using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct PlayerData
{
		public NetworkViewID id;
		public string name;
		public Color color;
		public int kills;
		public int deaths;
		public int assist;	
};

public class PlayerList : MonoBehaviour {
	
	List<PlayerData> playerList = new List<PlayerData>(2);
	
	[RPC]
	void RegisterPlayer(string playerName) {
		
		PlayerData player = new PlayerData();
		
		player.id = networkView.viewID;
		player.name = playerName;
		
		if (playerList.Count == 0)
			player.color = Color.red;
		else
			player.color = Color.blue;
		
		player.kills=0;
		player.deaths=0;
		player.assist=0;		
		
		playerList.Add(player);
	}
	
	[RPC]
	void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);
		
	}
}
