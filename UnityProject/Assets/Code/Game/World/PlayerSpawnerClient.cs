using UnityEngine;
using System.Collections;

public class PlayerSpawnerClient : MonoBehaviour {
	public GameObject playerPrefabClient;
	
	private NetworkViewID playerID;
	private PlayerList playerList;
	
	void Awake() {
		playerList = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>();
	}
	
	public void CreatePlayer(Team playerTeam) { 
		GameData.ACTUAL_CLIENT_TEAM = playerTeam;
		SpawnContolPlayer(playerTeam);
		networkView.RPC("SpawnPlayerOnOthers", RPCMode.Others, playerID, (int)playerTeam);
		playerList.CallRegisterPlayer(playerID, (int)playerTeam);
	}
	
	private void SpawnContolPlayer(Team playerTeam) {
		networkView.RPC("IncCounters", RPCMode.All, (int)playerTeam);
		playerID = AllocatePlayerID();
		Transform spawner = FindSpawn((Team)playerTeam);
		PlayerManager.instance.myPlayer = SpawnPlayer(playerID, spawner);
	}
	
	private NetworkViewID AllocatePlayerID() {
		return Network.AllocateViewID();
	}
	
	private Transform FindSpawn(Team playerTeam) {
		if (playerTeam == Team.TEAM_A) {
			Transform TA = this.transform.Find("TeamA");
			return TA.Find("Spawn1");
		}
		else if (playerTeam == Team.TEAM_B) {
			Transform TB = this.transform.Find("TeamB");
			return TB.Find("Spawn1");
		}
		else {
			Debug.LogError("Wrong spawn select");
			return this.transform;
		}	
	}
	
	private PlayerData SpawnPlayer(NetworkViewID playerID, Transform spawner) {
		GameObject player = Instantiate(playerPrefabClient, spawner.position, spawner.rotation) as GameObject;
		player.networkView.viewID = playerID;
		return player.GetComponent<PlayerData>();
	}
	
	[RPC]
	private void SpawnPlayerOnOthers(NetworkViewID playerID, int playerTeam) {
		Transform spawner = FindSpawn((Team)playerTeam);
		SpawnPlayer(playerID, spawner);
	}
	
	[RPC]
	private void IncCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TEAM_A)
			GameData.NUMBER_OF_PLAYERS_A++;
		else if ((Team)playerTeam == Team.TEAM_B)
			GameData.NUMBER_OF_PLAYERS_B++;
	}
	
	[RPC]
	private void DecCounters(int playerTeam) {
		if ((Team)playerTeam == Team.TEAM_A)
			GameData.NUMBER_OF_PLAYERS_A--;
		else if ((Team)playerTeam == Team.TEAM_B)
			GameData.NUMBER_OF_PLAYERS_B--;
	}
}