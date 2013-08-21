using UnityEngine;
using System.Collections;

public class PlayerSpawnerServer : MonoBehaviour {
	public GameObject playerPrefabServer;
	public GameObject spawners;
	
	void OnLevelWasLoaded(int level) {
		if(level == GameData.LEVEL_CRYSTAL_CAVERNS_SERVER) {
			spawners = GameObject.Find("Spawners");
		}
	}
	
	[RPC]
	private void SpawnPlayerOnOthers(NetworkViewID playerID, int playerTeam) {
		Transform spawner = FindSpawn((Team)playerTeam);
		Debug.LogWarning(spawner);
		SpawnPlayer(playerID, spawner);
	}
	
	private Transform FindSpawn(Team playerTeam) {
		Debug.LogWarning(spawners);
		if (playerTeam == Team.TeamA) {
			Transform TA = spawners.transform.FindChild("TeamA");
			return TA.FindChild("Spawn"+GameData.NUMBER_OF_PLAYERS_A);
		}
		else if (playerTeam == Team.TeamB) {
			Transform TB = spawners.transform.FindChild("TeamB");
			return TB.FindChild("Spawn"+GameData.NUMBER_OF_PLAYERS_B);
		}
		else {
			Debug.LogError("Wrong spawn select");
			return spawners.transform;
		}
	}
	
	private void SpawnPlayer(NetworkViewID playerID, Transform spawner) {
		GameObject player = Instantiate(playerPrefabServer, spawner.position, spawner.rotation) as GameObject;
		player.networkView.viewID = playerID;
	}
}
