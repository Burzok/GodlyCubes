using UnityEngine;
using System.Collections;

public class SpawnTower : MonoBehaviour {

	public Transform towerPrefabClient;
	public Transform towerPrefabServer;
	
	public Team teamSelect;
	
	public Transform tower;
	public NetworkViewID towerID;
	public NetworkViewID towerDetectorID;
	
	void Start () {
		if(Network.isServer) {
			AllocateNetworkID();
			//InstantiateTowerOnServer();
			//SetTeamInfoOnServer(teamSelect);
			networkView.RPC("InstantiateTowerOnClients", RPCMode.AllBuffered, towerID, towerDetectorID);
		}
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
	
	[RPC]
	private void InstantiateTowerOnClients(NetworkViewID towerID, NetworkViewID towerDetectorID) {
		if(Network.isServer)
			tower = Instantiate(towerPrefabServer, transform.position, transform.rotation) as Transform;
		else
			tower = Instantiate(towerPrefabClient, transform.position, transform.rotation) as Transform;
		
		//tower.networkView.viewID = towerID;
		//tower.GetChild(0).networkView.viewID = towerDetectorID;
		
		tower.GetComponent<NetworkView>().viewID = towerID;
		tower.GetChild(0).GetComponent<NetworkView>().viewID = towerDetectorID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnServer(Team team) {
		tower.GetComponent<TowerTeamChoserServer>().SetTeam(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		if(Network.isServer)
			tower.GetComponent<TowerTeamChoserServer>().SetTeam(teamSelect);
		else
			tower.GetComponent<TowerTeamChoserClient>().SetTeam(teamSelect);
	}
}
