using UnityEngine;
using System.Collections;

public class SpawnTowerServer : MonoBehaviour {

	public Transform towerPrefabServer;
	public Transform towerPrefabClient;
	public Team teamSelect;
	public Transform tower;
	
	private NetworkViewID towerID;
	private NetworkViewID towerDetectorID;
	
	void Awake () {
		networkView.group = 0;
		
		AllocateNetworkID();
		InstantiateTowerOnServer();
		SetTeamInfoOnServer(teamSelect);
	}
	
	private void AllocateNetworkID() {
		towerID = Network.AllocateViewID();	
		towerDetectorID = Network.AllocateViewID();	
	}
	
	private void InstantiateTowerOnServer() {
		tower = Instantiate(towerPrefabServer, transform.position, transform.rotation) as Transform;
		tower.networkView.viewID = towerID;
		tower.GetChild(0).networkView.viewID = towerDetectorID;
	}
	
	private void SetTeamInfoOnServer(Team team) {
		tower.GetComponent<TowerTeamChoserServer>().SetTeam(teamSelect);
	}
	
	public void SpawnTowerOnClient(NetworkPlayer sender) {
		networkView.RPC("InstantiateTowerOnClient", sender, towerID, towerDetectorID);
	}
	
	[RPC]
	private void InstantiateTowerOnClient(NetworkViewID towerID, NetworkViewID towerDetectorID) {
		tower = Instantiate(towerPrefabClient, transform.position, transform.rotation) as Transform;
		
		tower.networkView.viewID = towerID;
		tower.GetChild(0).networkView.viewID = towerDetectorID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		tower.GetComponent<TowerTeamChoserClient>().SetTeam(teamSelect);
	}
}
