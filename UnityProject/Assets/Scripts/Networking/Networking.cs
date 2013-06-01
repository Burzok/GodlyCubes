using UnityEngine;
using System.Collections;

public enum Team {
	TeamA,
	TeamB	
}

public class Networking : MonoBehaviour {
	public GameObject player_prefab;

	private string serverName = "Server Name";
	private string playerName = "Player Name";
	
	private GameObject goPlayer;	
	private GameObject spawners;
	
	public Team actualTeam;
	public int numOfPlayersA = 0;
	public int numOfPlayersB = 0;
	
	private MenuGUI mainMenu;
	
	void Awake () {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GodlyCubesLight");

		spawners = GameObject.Find("Spawners");
		mainMenu = GetComponent<MenuGUI>();
	}

	void OnConnectedToServer() {
		mainMenu.SetTeamSelectState();
	}
	
	public void ConnectToGame(Team playerTeam) { 
		Debug.Log(playerTeam);
		actualTeam = playerTeam;
		networkView.RPC("IncCounters", RPCMode.AllBuffered, (int)playerTeam);
		Transform spawner = FindSpawn(playerTeam);
		goPlayer = SpawnPlayer(ref spawner);
		networkView.RPC("RegisterPlayer", RPCMode.Server, playerName, getPlayerID(), (int)playerTeam);
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
	
	public Transform FindSpawn(Team playerTeam) {
		if (playerTeam == Team.TeamA) {
			Transform TA = spawners.transform.FindChild("TeamA");
			return TA.FindChild("Spawn"+numOfPlayersA);
		}
		else if (playerTeam == Team.TeamB) {
			Transform TB = spawners.transform.FindChild("TeamB");
			return TB.FindChild("Spawn"+numOfPlayersB);
		}
		else {
			Debug.Log("Error wrong spawn select");
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
		mainMenu.SetMainMenuState();
	}

	public NetworkViewID getPlayerID() {
		return goPlayer.networkView.viewID;
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