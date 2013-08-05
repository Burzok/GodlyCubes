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
		SetTeamInfoOnServer(ref teamSelect);
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
	
	private void SetTeamInfoOnServer(ref Team team) {
		tower.GetComponent<TowerTeamChoserServer>().SetTeam(ref teamSelect);
	}
	
	public void SpawnTowerOnClient(ref NetworkPlayer sender) {
		networkView.RPC("InstantiateTowerOnClient", sender, towerID, towerDetectorID);
	}
	
	public void SpawnTowerBulletOnClient(ref NetworkPlayer sender, NetworkViewID bulletID) {
		networkView.RPC("InstantiateTowerBulletOnClient", sender, bulletID);
	}
	
	[RPC]
	private void InstantiateTowerOnClient(NetworkViewID towerID, NetworkViewID towerDetectorID) 
	{}
	
	[RPC]
	private void InstantiateTowerBulletOnClient(NetworkViewID bulletID) 
	{}
}
