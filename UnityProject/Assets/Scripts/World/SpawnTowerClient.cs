using UnityEngine;
using System.Collections;

public class SpawnTowerClient : MonoBehaviour {

	public Transform towerPrefabClient;
	public Team teamSelect;
	public Transform tower;
	
	[RPC]
	private void InstantiateTowerOnClients(NetworkViewID towerID, NetworkViewID towerDetectorID) {

		tower = Instantiate(towerPrefabClient, transform.position, transform.rotation) as Transform;
		
		//tower.networkView.viewID = towerID;
		//tower.GetChild(0).networkView.viewID = towerDetectorID;
		
		tower.GetComponent<NetworkView>().viewID = towerID;
		tower.GetChild(0).GetComponent<NetworkView>().viewID = towerDetectorID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		tower.GetComponent<TowerTeamChoserClient>().SetTeam(teamSelect);
	}
}
