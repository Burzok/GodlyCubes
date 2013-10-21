using UnityEngine;
using System.Collections;

public class SpawnBaseServer : MonoBehaviour {

	public Transform basePrefabServer;
	public Team teamSelect;
	
	public Transform gameBase;		// chenge to private after tests
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
		gameBase.GetComponent<BaseTeamChoserServer>().SetTeam(ref teamSelect);
	}
	
	public void SpawnBaseOnClient(NetworkPlayer sender) {
		networkView.RPC("InstantiateBaseOnClient", sender, baseID);
	}
	
	[RPC]
	private void InstantiateBaseOnClient(NetworkViewID baseID) 
	{}
}
