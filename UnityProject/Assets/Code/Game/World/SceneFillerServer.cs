using UnityEngine;
using System.Collections;

public class SceneFillerServer : MonoBehaviour {
	
	[RPC]
	private void WhatToSpawnOnClient(NetworkMessageInfo info) {
		NetworkPlayer sender = info.sender;
		
		PlayersSpawn(ref sender);
		TowersSpawn(ref sender);
		TowersBulletSpawn(ref sender);
		CrystalSpawn(ref sender);
		BasesSpawn(ref sender);
		ObstaclesSpawn(ref sender);
	}
	
	private void PlayersSpawn(ref NetworkPlayer sender) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		
		if(players.Length == 0)
			networkView.RPC("NoPlayersToSpawn", sender);
		else
			SendSpawnOtherPlayersOnClientCommand(ref sender, ref players);
	}
	
	private void SendSpawnOtherPlayersOnClientCommand(ref NetworkPlayer sender, ref GameObject[] players) {
		foreach(GameObject player in players) {
			if(player.networkView.owner != sender) { // nie ma jeszcze takiego ale w razie wu
				int numberOfPlayersToSpawn = players.Length;
				NetworkViewID playerID = player.networkView.viewID;
				Vector3 spawnPosition = player.GetComponent<PlayerData>().respawnPosition;
                int playerTeam = (int)player.GetComponent<PlayerData>().team;

                Vector3 playerBackColor = player.GetComponent<PlayerData>().color;

				networkView.RPC("SpawnOtherPlayersOnClient", sender, 
                    playerID, spawnPosition, playerTeam, playerBackColor,  numberOfPlayersToSpawn);
			}
		}
	}
	
	[RPC]
	private void NoPlayersToSpawn() 
	{}
	
	[RPC]
	private void SpawnOtherPlayersOnClient(
        NetworkViewID playerID, Vector3 spawnPosition, int playerTeam, Vector3 playerBackColor, int numberOfPlayersToSpawn) 
	{}
	
	[RPC]
	private void DecPlayersConnectingNumber() {
		GameData.NUMBER_OF_CONNECTING_PLAYERS--;
	}
	
	[RPC]
	private void TurnOnPlayersNetworkViewsOnServer(NetworkMessageInfo info) {
		if(GameData.NUMBER_OF_CONNECTING_PLAYERS == 0) {
			GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
			
			if(players.Length != 0)
				TurnOnNetworkViewsOff(ref players);
			
			networkView.RPC("FinishConnecting", RPCMode.All);
			networkView.RPC("SetTeamSelectStateOnClient", info.sender);
		}
	}
	
	private void TurnOnNetworkViewsOff(ref GameObject[] players) {
		foreach(GameObject player in players) {
			player.networkView.enabled = true;
		}
	}
	
	[RPC]
	private void FinishConnecting() {
		GameTime.UnPauseGame();
		ServerManager.instance.HidePlayersConnectingPopup();
	}
	
	[RPC]
	private void SetTeamSelectStateOnClient() 
	{}
	
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
			if(crystalSpawner.crystal)
					crystalSpawner.SpawnCrystalOnClient(ref sender);
		}
	}
	
	private void BasesSpawn(ref NetworkPlayer sender) {
		GameObject[] bases = GameObject.FindGameObjectsWithTag(Tags.baseSpawner);
		foreach(GameObject gameBase in bases) {
			SpawnBaseServer baseSpawner = gameBase.GetComponent<SpawnBaseServer>();
			baseSpawner.SpawnBaseOnClient(sender);
		}
	}

	private void ObstaclesSpawn(ref NetworkPlayer sender) {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag(Tags.obstacleSpawner);
		foreach(GameObject obstacle in obstacles) {
			ObstacleSpawnerServer obstacleSpawner = obstacle.GetComponent<ObstacleSpawnerServer>();
			obstacleSpawner.SpawnObstacleOnClient(sender);
		}
	}
}
