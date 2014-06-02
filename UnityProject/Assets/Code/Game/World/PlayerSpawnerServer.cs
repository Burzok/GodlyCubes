using UnityEngine;
using System.Collections;

public class PlayerSpawnerServer : MonoBehaviour {
	public GameObject playerPrefabServer;
	
	[RPC]
	private void SpawnPlayerOnOthers(NetworkViewID playerID, int playerTeam) {
		Transform spawner = FindSpawn((Team)playerTeam);
		SpawnPlayer(playerID, spawner);
	}
	
	private Transform FindSpawn(Team playerTeam) {
		if (playerTeam == Team.TEAM_A) {
			Transform TA = this.transform.Find("TeamA");
			return TA.FindChild("Spawn1");
		}
		else if (playerTeam == Team.TEAM_B) {
			Transform TB = this.transform.Find("TeamB");
			return TB.FindChild("Spawn1");
		}
		else {
			Debug.LogError("Wrong spawn select");
			return this.transform;
		}
	}
	
	private void SpawnPlayer(NetworkViewID playerID, Transform spawner) {
		GameObject player = Instantiate(playerPrefabServer, spawner.position, spawner.rotation) as GameObject;
		player.networkView.viewID = playerID;
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
