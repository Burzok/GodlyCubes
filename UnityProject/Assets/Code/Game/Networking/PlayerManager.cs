using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : SingletonComponent<PlayerManager> {

	[SerializeField] public PlayerData myPlayer;
	[SerializeField] public List<PlayerData> playerDataList = new List<PlayerData>();
	
	public void AddPlayer(PlayerData p) {
		if(playerDataList.Contains(p) == false)
			playerDataList.Add(p);
	}
	
	public void RemovePlayer(PlayerData p) {
		playerDataList.Remove(p);
	}
	
	public PlayerData ReturnPlayerData(string playerName) {
		foreach(PlayerData playerData in playerDataList) {
			if (playerData.playerName == playerName)
				return playerData;
		}
		
		Debug.LogError("No Player of name: " + playerName);
		return null;
	}
	
	public Transform ReturnPlayerTransform(string playerName) {
		return ReturnPlayerData(playerName).transform;
	}
	
	public PlayerStats ReturnPlayerStats(string playerName) {
		return ReturnPlayerData(playerName).stats;
	}
}
