using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Team {
	TeamA,
	TeamB,
	Neutral
}

public class Networking : MonoBehaviour {
	
	public string disconnectedLevel;
	
	private MenuGUI mainMenu;
	private PlayerList playerListComponent;
	private PlayerSpawnerClient playerSpawner;
	
	void Awake () {
		GameData.SERVER_NAME = "Server Name";
		GameData.PLAYER_NAME = "Player Name";
		
		GameData.NUMBER_OF_PLAYERS_A = 0;
		GameData.NUMBER_OF_PLAYERS_A = 0;
		
		disconnectedLevel = "main_menu";
		
		DontDestroyOnLoad(this);
		
		networkView.group = 1;
		Application.LoadLevel(disconnectedLevel);
		
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");

		mainMenu = GetComponent<MenuGUI>();
		playerListComponent = GetComponent<PlayerList>();
	}
	
	void OnConnectedToServer() {
		Network.SetSendingEnabled(0, false);
		foreach (NetworkPlayer player in Network.connections)  
         	Network.SetReceivingEnabled(player, 0, false);
	
		mainMenu.SetTeamSelectState();
	}	
	
	void OnDisconnectedFromServer () {
		GameObject []players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			GameObject.Destroy(player);					
		}
		GetComponent<PlayerList>().playerList.Clear();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {// TODO: Pytanie: czy ta funkcja jest bezurzyteczna obecnie?
		//Network.RemoveRPCs(player);
		//Network.DestroyPlayerObjects(player);
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
	
	public void ConnectToGame(Team playerTeam) {
		playerSpawner.CreatePlayer(playerTeam);
	}
}