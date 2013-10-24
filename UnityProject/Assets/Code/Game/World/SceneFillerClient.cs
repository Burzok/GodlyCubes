using UnityEngine;
using System.Collections;

public class SceneFillerClient : MonoBehaviour {
	public GameObject playerPrefabClient;
	public GameObject testPlayer; // zmienna testowa do podgladu, jesli wszystko dziala to usunac
	
	private int otherPlayersSpawned;
    private TeamSelectUI teamSelectUI;
	
	void Awake() {
        teamSelectUI = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<TeamSelectUI>();
		otherPlayersSpawned = 0;
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_CRYSTAL_CAVERNS_CLIENT) {
			networkView.RPC("WhatToSpawnOnClient", RPCMode.Server);
		}
	}
	
	[RPC]
	private void WhatToSpawnOnClient(NetworkMessageInfo info)
	{}
	
	[RPC]
	private void NoPlayersToSpawn() {
		networkView.RPC("DecPlayersConnectingNumber", RPCMode.Others);
		networkView.RPC("TurnOnPlayersNetworkViewsOnServer", RPCMode.Server);
	}
	
	[RPC]
	private void SpawnOtherPlayersOnClient(NetworkViewID playerID, Vector3 spawnPosition, int numberOfPlayersToSpawn) {
		otherPlayersSpawned++;
		
		testPlayer = Instantiate(playerPrefabClient, spawnPosition, Quaternion.identity) as GameObject;
		testPlayer.networkView.viewID = playerID;
		
		if(otherPlayersSpawned == numberOfPlayersToSpawn) {
			networkView.RPC("DecPlayersConnectingNumber", RPCMode.Others);
			networkView.RPC("TurnOnPlayersNetworkViewsOnServer", RPCMode.Server);
		}
	}
	
	[RPC]
	private void DecPlayersConnectingNumber() {
		GameData.NUMBER_OF_CONNECTING_PLAYERS--;
	}
	
	[RPC]
	private void TurnOnPlayersNetworkViewsOnServer() 
	{}
	
	[RPC]
	private void FinishConnecting() {
		GameTime.UnPauseGame();
		ServerManager.instance.HidePlayersConnectingPopup();
	}
	
	[RPC]
	private void SetTeamSelectStateOnClient() {
		teamSelectUI.SetTeamSelectState();
	}
}
