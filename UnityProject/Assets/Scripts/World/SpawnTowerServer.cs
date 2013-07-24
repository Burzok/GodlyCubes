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
		Debug.LogWarning("Awake "+networkView.group);
		networkView.RPC("InstantiateTowerOnClients", RPCMode.OthersBuffered, towerID, towerDetectorID);
	}
	
	void Start() {
		Debug.LogWarning("Start "+networkView.group);
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
