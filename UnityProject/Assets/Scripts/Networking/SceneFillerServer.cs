using UnityEngine;
using System.Collections;

public class SceneFillerServer : MonoBehaviour {
	
	public GameObject playerPrefabClient;
	public GameObject newPlayer;
	
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
		
		newPlayer = Instantiate(playerPrefabClient, spawnPosition, Quaternion.identity) as GameObject;
		newPlayer.networkView.viewID = playerID;
	}
}
