using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Team {
	TeamA,
	TeamB,
	Neutral
}

public class Networking : MonoBehaviour {
	public GameObject player_prefab;
	public Team actualTeam;
	
	private string serverName;
	private string playerName;
	
	private GameObject spawners;
	
	private int numOfPlayersA;
	private int numOfPlayersB;
	
	private MenuGUI mainMenu;
	private PlayerList playerListComponent;
	
	public string disconnectedLevel;
	
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
		if(level == 3) {
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
		CrystalSpawn(ref sender);
		//BasesSpawn(ref sender);
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
	
	private void CrystalSpawn(ref NetworkPlayer sender) {
		GameObject[] crystals = GameObject.FindGameObjectsWithTag(Tags.crystalSpawner);
		foreach(GameObject crystal in crystals) {
			CrystalSpawnerServer crystalSpawner = crystal.GetComponent<CrystalSpawnerServer>();
			if(crystalSpawner.crystal) {	
					crystalSpawner.SpawnCrystalOnClient(ref sender);
			}
		}
	}
	
	private void BasesSpawn(ref NetworkPlayer sender) {
		GameObject[] bases = GameObject.FindGameObjectsWithTag(Tags.baseSpawner);
		foreach(GameObject gameBase in bases) {
			//SpawnBaseServer baseSpawner = gameBase.GetComponent<SpawnBaseServer>();
			//gameBase.SpawnBaseOnClient(sender);
		}
	}

	void OnConnectedToServer() {
		Network.SetSendingEnabled(0, false);
		foreach (NetworkPlayer player in Network.connections)  
         	Network.SetReceivingEnabled(player, 0, false);
	
		mainMenu.SetTeamSelectState();
	}
	
	public void ConnectToGame(Team playerTeam) { 
		actualTeam = playerTeam;
		networkView.RPC("IncCounters", RPCMode.AllBuffered, (int)playerTeam);
		
		Transform spawner = FindSpawn(playerTeam);
		playerListComponent.myPlayer = SpawnPlayer(ref spawner);
		
		networkView.RPC("RegisterPlayer", RPCMode.AllBuffered, playerName, getMyPlayerID(), (int)playerTeam);
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
	
	private GameObject SpawnPlayer(ref Transform spawner) {
		return Network.Instantiate(player_prefab, spawner.transform.position, spawner.transform.rotation, 0) 
			as GameObject;
	}
	
	private Transform FindSpawn(Team playerTeam) {
		Debug.Log (spawners);
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