using UnityEngine;
using System.Collections;

public class SpawnTowerClient : MonoBehaviour {

	public Transform towerPrefabClient;
	public Team teamSelect;
	public Transform tower;
	
	void Awake() {
		networkView.group = 0;
		Debug.LogWarning("Awake "+networkView.group);
	}
	
	void Start() {
		Debug.LogWarning(networkView.group);
	}
	
	[RPC]
	private void InstantiateTowerOnClients(NetworkViewID towerID, NetworkViewID towerDetectorID, NetworkMessageInfo info) {
		Debug.LogWarning("Sender group: "+info.networkView.group);
		
		tower = Instantiate(towerPrefabClient, transform.position, transform.rotation) as Transform;
		
		tower.networkView.viewID = towerID;
		tower.GetChild(0).networkView.viewID = towerDetectorID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		tower.GetComponent<TowerTeamChoserClient>().SetTeam(teamSelect);
	}
}
