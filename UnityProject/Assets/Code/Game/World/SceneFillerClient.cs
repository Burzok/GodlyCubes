using UnityEngine;
using System.Collections;

public class SceneFillerClient : MonoBehaviour {
	[SerializeField] private GameObject playerPrefabClient;
    [SerializeField] private GameObject testPlayer; // zmienna testowa do podgladu, jesli wszystko dziala to usunac
	
	private int otherPlayersSpawned;
	
	void Awake() {
		otherPlayersSpawned = 0;
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_DEATH_MATCH_CLIENT) {
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
	private void SpawnOtherPlayersOnClient(
            NetworkViewID playerID, Vector3 spawnPosition, int playerTeam, Vector3 playerBackColor, string playerName, int numberOfPlayersToSpawn)
    {

		otherPlayersSpawned++;
		
		testPlayer = Instantiate(playerPrefabClient, spawnPosition, Quaternion.identity) as GameObject;
		testPlayer.networkView.viewID = playerID;
        testPlayer.rigidbody.isKinematic = true;

        PlayerData data = testPlayer.GetComponent<PlayerData>();

        if ((Team)playerTeam == Team.TEAM_A) {
            testPlayer.layer = PhysicsLayers.LAYER_TEAM_A;
            data.color = new Vector3(1, 0, 0);
            data.team = Team.TEAM_A;
        }
        else if ((Team)playerTeam == Team.TEAM_B) {
            testPlayer.layer = PhysicsLayers.LAYER_TEAM_B;
            data.color = new Vector3(0, 0, 1);
            data.team = Team.TEAM_B;
        }

		data.id = playerID;
		data.playerName = playerName;

        Transform playerArmature = testPlayer.transform.Find("Animator");
        Renderer[] playerRenderers = playerArmature.GetComponentsInChildren<Renderer>();
        float tuning = 0.5f;

        foreach (Renderer rend in playerRenderers) {
            if (rend.gameObject.name == "Player")
                rend.material.color = new Color(data.color.x * tuning, data.color.y * tuning, data.color.z * tuning);
            else if (rend.gameObject.name == "Bullet")
                rend.material.color = new Color(data.color.x, data.color.y, data.color.z);
            else
                rend.material.color = new Color(playerBackColor.x, playerBackColor.y, playerBackColor.z);
        }

        data.color = playerBackColor;

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

	}
}
