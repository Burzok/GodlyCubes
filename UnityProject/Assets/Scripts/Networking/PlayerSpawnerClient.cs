using UnityEngine;
using System.Collections;

public class PlayerSpawnerClient : MonoBehaviour {
	public GameObject playerPrefabClient;
	
	private GameObject spawners;
	private NetworkViewID playerID;
	private PlayerList playerList;
	
	void Awake() {
		playerList = GetComponent<PlayerList>();
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_CRYSTAL_CAVERNS_CLIENT) {
			spawners = GameObject.Find("Spawners");
			networkView.RPC("WhatToSpawnOnClient", RPCMode.Server);
		}
	}
	
	public void CreatePlayer(Team playerTeam) { 
		GameData.ACTUAL_CLIENT_TEAM = playerTeam;
		SpawnContolPlayer(playerTeam);
		networkView.RPC("SpawnPlayerOnOthers", RPCMode.Others, (int)playerTeam);
		networkView.RPC("RegisterPlayer", RPCMode.All, GameData.PLAYER_NAME, playerID, playerTeam);
	}
	
	private void SpawnContolPlayer(Team playerTeam) {
		networkView.RPC("IncCounters", RPCMode.All, (int)playerTeam);
		playerID = AllocatePlayerID();
		Transform spawner = FindSpawn((Team)playerTeam);
		
		Debug.LogWarning(spawner);
		
		playerList.myPlayer = SpawnPlayer(playerID, spawner);
	}
	
	private NetworkViewID AllocatePlayerID() {
		return Network.AllocateViewID();
	}
	
	private Transform FindSpawn(Team playerTeam) {
		Debug.LogWarning(spawners);
		
		if (playerTeam == Team.TeamA) {
			Transform TA = spawners.transform.FindChild("TeamA");
			return TA.FindChild("Spawn"+GameData.NUMBER_OF_PLAYERS_A);
		}
		else if (playerTeam == Team.TeamB) {
			Transform TB = spawners.transform.FindChild("TeamB");
			return TB.FindChild("Spawn"+GameData.NUMBER_OF_PLAYERS_B);
		}
		else {
			Debug.LogError("Wrong spawn select");
			return spawners.transform;
		}
	}
	
	private GameObject SpawnPlayer(NetworkViewID playerID, Transform spawner) {
		GameObject player = Instantiate(playerPrefabClient, spawner.position, spawner.rotation) as GameObject;
		player.networkView.viewID = playerID;
		return player;
	}
	
	[RPC]
	private void SpawnPlayerOnOthers(NetworkViewID playerID, int playerTeam) {
		Transform spawner = FindSpawn((Team)playerTeam);
		Debug.LogWarning(spawner);
		SpawnPlayer(playerID, spawner);
	}
	
	[RPC]
	private void IncCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TeamA)
			GameData.NUMBER_OF_PLAYERS_A++;
		else if ((Team)playerTeam == Team.TeamB)
			GameData.NUMBER_OF_PLAYERS_B++;
	}
	
	[RPC]
	private void DecCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TeamA)
			GameData.NUMBER_OF_PLAYERS_A--;
		else if ((Team)playerTeam == Team.TeamB)
			GameData.NUMBER_OF_PLAYERS_B--;
	}
}
