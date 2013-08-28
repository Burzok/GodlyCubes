using UnityEngine;
using System.Collections;

public class SceneFillerServer : MonoBehaviour {
	
	private MenuUI menuGUI;
	
	void Awake() {
		menuGUI = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<MenuUI>();
	}
	
	[RPC]
	private void WhatToSpawnOnClient(NetworkMessageInfo info) {
		NetworkPlayer sender = info.sender;
		
		PlayersSpawn(ref sender);
		TowersSpawn(ref sender);
		TowersBulletSpawn(ref sender);
		CrystalSpawn(ref sender);
		//BasesSpawn(ref sender);
	}
	
	private void PlayersSpawn(ref NetworkPlayer sender) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		
		if(players.Length == 0) {
			networkView.RPC("NoPlayersToSpawn", sender);
		}
		else {
			SendSpawnOtherPlayersOnClientCommand(ref sender, ref players);
		}
	}
	
	private void SendSpawnOtherPlayersOnClientCommand(ref NetworkPlayer sender, ref GameObject[] players) {
		foreach(GameObject player in players) {
			if(player.networkView.owner != sender) { // nie ma jeszcze takiego ale w razie wu
				int numberOfPlayersToSpawn = players.Length;
				NetworkViewID playerID = player.networkView.viewID;
				Vector3 spawnPosition = player.GetComponent<PlayerData>().respawnPosition;
				networkView.RPC("SpawnOtherPlayersOnClient", sender, playerID, spawnPosition, numberOfPlayersToSpawn);
			}
		}
	}
	
	[RPC]
	private void NoPlayersToSpawn() 
	{}
	
	[RPC]
	private void SpawnOtherPlayersOnClient(NetworkViewID playerID, Vector3 spawnPosition, int numberOfPlayersToSpawn) 
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
		menuGUI.HidePlayersConnectingPopup();
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
		//GameObject[] bases = GameObject.FindGameObjectsWithTag(Tags.baseSpawner);
		//foreach(GameObject gameBase in bases) {
			//SpawnBaseServer baseSpawner = gameBase.GetComponent<SpawnBaseServer>();
			//gameBase.SpawnBaseOnClient(sender);
		//}
	}
}
