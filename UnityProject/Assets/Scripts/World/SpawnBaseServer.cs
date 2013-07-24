using UnityEngine;
using System.Collections;

public class SpawnBaseServer : MonoBehaviour {

	public Transform basePrefabServer;
	public Transform basePrefabClient;
	public Team teamSelect;
	public Transform gameBase;
	
	private NetworkViewID baseID;
	
	void Awake () {
		networkView.group = 0;
		
		AllocateNetworkID();
		InstantiateBaseOnServer();
		SetTeamInfoOnServer(teamSelect);
	}
	
	private void AllocateNetworkID() {
		baseID = Network.AllocateViewID();	
	}
	
	private void InstantiateBaseOnServer() {
		gameBase = Instantiate(basePrefabServer, transform.position, transform.rotation) as Transform;
		gameBase.networkView.viewID = baseID;
	}
	
	private void SetTeamInfoOnServer(Team team) {
		//base.GetComponent<BaseTeamChoserServer>().SetTeam(teamSelect);
	}
	
	public void SpawnBaseOnClient(NetworkPlayer sender) {
		networkView.RPC("InstantiateBaseOnClient", sender, baseID);
	}
	
	[RPC]
	private void InstantiateBaseOnClient(NetworkViewID baseID) {
		gameBase = Instantiate(basePrefabClient, transform.position, transform.rotation) as Transform;
		gameBase.networkView.viewID = baseID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		//base.GetComponent<BaseTeamChoserClient>().SetTeam(teamSelect);
	}
}
