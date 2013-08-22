using UnityEngine;
using System.Collections;

public class SceneFillerClient : MonoBehaviour {
	public GameObject playerPrefabClient;
	public GameObject newPlayer; // zmienna testowa do podgladu, jesli wszystko dziala to usunac
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_CRYSTAL_CAVERNS_CLIENT)
			networkView.RPC("WhatToSpawnOnClient", RPCMode.Server);
	}
	
	[RPC]
	private void WhatToSpawnOnClient(NetworkMessageInfo info)
	{}
	
	[RPC]
	private void SpawnOtherPlayersOnClient(NetworkViewID playerID, Vector3 spawnPosition) {
		newPlayer = Instantiate(playerPrefabClient, spawnPosition, Quaternion.identity) as GameObject;
		newPlayer.networkView.viewID = playerID;
	}
}
