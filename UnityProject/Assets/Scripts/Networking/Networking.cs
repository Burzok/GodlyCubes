using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Team {
	TeamA,
	TeamB,
	Neutral
}

public class Networking : MonoBehaviour {
	public GameObject playerPrefabServer;
	public GameObject playerPrefabClient;
	public Team actualTeam;
	
	private string serverName;
	private string playerName;
	
	public GameObject spawners;
	
	private int numOfPlayersA;
	private int numOfPlayersB;
	
	private MenuGUI mainMenu;
	private PlayerList playerListComponent;
	
	public string disconnectedLevel;
	
	public GameObject NewPlayer;
	
	void Awake () {
		serverName = "Server Name";
		playerName = "Player Name";
		numOfPlayersA = 0;
		numOfPlayersB = 0;
		disconnectedLevel = "main_menu";
		
		DontDestroyOnLoad(this);
		networkView.group = 1;
		Application.LoadLevel(disconnectedLevel);
		
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");

		mainMenu = GetComponent<MenuGUI>();
		playerListComponent = GetComponent<PlayerList>();
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == 2) {
			spawners = GameObject.Find("Spawners");
		}
		else if(level == 3) {
			spawners = GameObject.Find("Spawners");
			networkView.RPC("WhatToSpawnOnClient", RPCMode.Server);
		}
	}
	
	[RPC]
	private void WhatToSpawnOnClient(NetworkMessageInfo info) {
		Debug.LogWarning("Server caches WhatToSpawnOnClient");
		NetworkPlayer sender = info.sender;
		
		TowersSpawn(ref sender);
		TowersBulletSpawn(ref sender);
		//BasesSpawn(ref sender);
		PlayersSpawn(ref sender);
	}
	
	private void TowersSpawn(ref NetworkPlayer sender) {
		GameObject[] towers = GameObject.FindGameObjectsWithTag(Tags.towerSpawner);
		foreach(GameObject tower in towers) {
			SpawnTowerServer towerSpawner = tower.GetComponent<SpawnTowerServer>();
			if(towerSpawner.tower)
				if(towerSpawner.tower.GetComponent<TowerLifeServer>().isAlive)
					towerSpawner.SpawnTowerOnClient(ref sender);
		}
	}
	
	private void TowersBulletSpawn(ref NetworkPlayer sender) {
		GameObject[] towers = GameObject.FindGameObjectsWithTag(Tags.towerSpawner);
		foreach(GameObject tower in towers) {
			SpawnTowerServer towerSpawner = tower.GetComponent<SpawnTowerServer>();
			if(towerSpawner.tower) {	
				TowerReloaderServer reloader = towerSpawner.tower.GetComponent<TowerReloaderServer>();
				if(reloader.bullet)
					towerSpawner.SpawnTowerBulletOnClient(ref sender, reloader.bullet.networkView.viewID);
			}
		}
	}
	
	private void BasesSpawn(ref NetworkPlayer sender) {
		//GameObject[] bases = GameObject.FindGameObjectsWithTag(Tags.baseSpawner);
		//foreach(GameObject gameBase in bases) {
			//SpawnBaseServer baseSpawner = gameBase.GetComponent<SpawnBaseServer>();
			//gameBase.SpawnBaseOnClient(sender);
		//}
	}
	
	private void PlayersSpawn(ref NetworkPlayer sender) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			if(player.networkView.owner != sender) {
				Debug.LogWarning("Serwer sends do client to spawn serwer");
				NetworkViewID playerID = player.networkView.viewID;
				Vector3 spawnPosition = player.GetComponent<PlayerData>().respawnPosition;
				networkView.RPC("SpawnOtherPlayersOnClient", sender, playerID, spawnPosition);
			}
		}
	}
	
	[RPC]
	private void SpawnOtherPlayersOnClient(NetworkViewID playerID, Vector3 spawnPosition) {
		Debug.LogWarning("Client caches player spawn from serwer");
		NewPlayer = Instantiate(playerPrefabClient, spawnPosition, Quaternion.identity) as GameObject;
		NewPlayer.networkView.viewID = playerID;
	}

	void OnConnectedToServer() {
		Network.SetSendingEnabled(0, false);
		foreach (NetworkPlayer player in Network.connections)  
         	Network.SetReceivingEnabled(player, 0, false);
	
		mainMenu.SetTeamSelectState();
	}
	
	public void ConnectToGame(Team playerTeam) { 
		actualTeam = playerTeam;
		networkView.RPC("SpawnPlayerOnServer", RPCMode.Server, (int)playerTeam);
	}
	
	[RPC]
	private void SpawnPlayerOnServer(int playerTeam) {
		networkView.RPC("IncCounters", RPCMode.All, (int)playerTeam);
		NetworkViewID playerID = AllocatePlayerID();
		Transform spawner = FindSpawn((Team)playerTeam);
		Debug.LogWarning(spawner);
		SpawnPlayer(playerID, spawner);
		networkView.RPC("SpawnPlayerOnClients", RPCMode.Others, playerID, playerTeam, spawner.position);
		networkView.RPC("RegisterPlayer", RPCMode.All, playerName, playerID, playerTeam);
	}
	
	private NetworkViewID AllocatePlayerID() {
		return Network.AllocateViewID();
	}
	
	private Transform FindSpawn(Team playerTeam) {
		Debug.LogWarning(spawners);
		if (playerTeam == Team.TeamA) {
			Transform TA = spawners.transform.FindChild("TeamA");
			return TA.FindChild("Spawn"+numOfPlayersA);
		}
		else if (playerTeam == Team.TeamB) {
			Transform TB = spawners.transform.FindChild("TeamB");
			return TB.FindChild("Spawn"+numOfPlayersB);
		}
		else {
			Debug.LogError("Wrong spawn select");
			return spawners.transform;
		}
	}
	
	private void SpawnPlayer(NetworkViewID playerID, Transform spawner) {
		GameObject player = Instantiate(playerPrefabServer, spawner.position, spawner.rotation) as GameObject;
		player.networkView.viewID = playerID;
	}
	
	[RPC]
	private void SpawnPlayerOnClients(NetworkViewID playerID, int playerTeam, Vector3 spawnerPosition) {
		GameObject player = Instantiate(playerPrefabClient, spawnerPosition, Quaternion.identity) as GameObject;
		player.networkView.viewID = playerID;
	}
			
	private void OldConnect(Team playerTeam) {
		actualTeam = playerTeam;
		networkView.RPC("IncCounters", RPCMode.All, (int)playerTeam);
		
		Transform spawner = FindSpawn(playerTeam);
		//playerListComponent.myPlayer = SpawnPlayerOnClient(ref spawner);
		//AllocatePlayerID();
		//playerListComponent.myPlayer.networkView.viewID = 
		
		//networkView.RPC("SpawnPlayerOnOthers", RPCMode.Others);
		networkView.RPC("RegisterPlayer", RPCMode.All, playerName, getMyPlayerID(), (int)playerTeam);
	}
	
	[RPC]
	private void IncCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TeamA)
			numOfPlayersA++;
		else if ((Team)playerTeam == Team.TeamB)
			numOfPlayersB++;
	}
	
	[RPC]
	private void DecCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TeamA)
			numOfPlayersA--;
		else if ((Team)playerTeam == Team.TeamB)
			numOfPlayersB--;
	}
	
	void OnDisconnectedFromServer () {
		GameObject []players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			GameObject.Destroy(player);					
		}
		GetComponent<PlayerList>().playerList.Clear();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
 	
	[RPC]
	void ExitCL() {
  		Network.Disconnect();
		//mainMenu.SetMainMenuState();
		Application.LoadLevel(0);
	}

	public NetworkViewID getMyPlayerID() {
		return playerListComponent.myPlayer.networkView.viewID;
	}
	
	public void SetServerName(string serwerName) {
		this.serverName = serwerName;
	}
	
	public string GetServerName() {
		return serverName;
	}
	
	public void SetPlayerName(string playerName) {
		this.playerName = playerName;
	}
	
	public string GetPlayerName() {
		return playerName;
	}
}