using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {

	private PlayerSpawnerClient playerSpawnerComponent;
	
	void Awake () {
		GameData.SERVER_NAME = "Server Name";
		GameData.PLAYER_NAME = "Player Name";
		
		GameData.NUMBER_OF_PLAYERS_A = 0;
		GameData.NUMBER_OF_PLAYERS_A = 0;
		
		DontDestroyOnLoad(this);
		
		networkView.group = 1;
		Application.LoadLevel(GameData.LEVEL_DISCONNECTED);
		
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight"); //TODO: globalna zmienna w GameDAta
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_DEATH_MATCH_CLIENT)
			playerSpawnerComponent = GameObject.Find("SpawnersClient").GetComponent<PlayerSpawnerClient>();
	}
	
	void OnConnectedToServer() {
		Network.SetSendingEnabled(0, false);
		foreach (NetworkPlayer player in Network.connections) {
         	Network.SetReceivingEnabled(player, 0, false);
		}
		ServerManager.instance.AddInitializeConnecting();
	}	
	
	void OnPlayerConnected() {
		TurnPlayersNetworkViewOff();
	}
	
	private void TurnPlayersNetworkViewOff() {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			player.networkView.enabled = false;
		}
	}
	
	void OnDisconnectedFromServer () {
		GetComponent<PlayerList>().playerList.Clear();
	}

	public NetworkViewID getMyPlayerID() {
		return PlayerManager.instance.myPlayer.networkView.viewID;
	}
	
	public void ConnectToGame(Team playerTeam) {
		playerSpawnerComponent.CreatePlayer(playerTeam);
	}
}