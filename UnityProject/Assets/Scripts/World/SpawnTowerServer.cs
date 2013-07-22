using UnityEngine;
using System.Collections;

public class SpawnTowerServer : MonoBehaviour {

	public Transform towerPrefabServer;
	
	public Team teamSelect;
	
	public Transform tower;
	private NetworkViewID towerID;
	private NetworkViewID towerDetectorID;
	
	void Start () {
		AllocateNetworkID();
		InstantiateTowerOnServer();
		SetTeamInfoOnServer(teamSelect);
		networkView.RPC("InstantiateTowerOnClients", RPCMode.OthersBuffered, towerID, towerDetectorID);
	}
	
	private void AllocateNetworkID() {
		towerID = Network.AllocateViewID();	
		towerDetectorID = Network.AllocateViewID();	
	}
	
	private void InstantiateTowerOnServer() {
		tower = Instantiate(towerPrefabServer, transform.position, transform.rotation) as Transform;
		tower.GetComponent<NetworkView>().viewID = towerID;
		tower.GetChild(0).GetComponent<NetworkView>().viewID = towerDetectorID;
	}
	
	private void SetTeamInfoOnServer(Team team) {
		tower.GetComponent<TowerTeamChoserServer>().SetTeam(teamSelect);
	}
	
	[RPC]
	private void InstantiateTowerOnClients(NetworkViewID towerID, NetworkViewID towerDetectorID) {

		tower = Instantiate(towerPrefabServer, transform.position, transform.rotation) as Transform;
		
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
