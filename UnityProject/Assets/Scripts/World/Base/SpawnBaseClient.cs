using UnityEngine;
using System.Collections;

public class SpawnBaseClient : MonoBehaviour {

	public Transform basePrefabClient;
	public Team teamSelect;
	
	public GameObject gameBase; // chenge to private after tests
	
	[RPC]
	private void InstantiateBaseOnClient(NetworkViewID baseID) {
		gameBase = Instantiate(basePrefabClient, transform.position, transform.rotation) as GameObject;
		gameBase.networkView.viewID = baseID;
		
		SetTeamInfoOnClient();
	}
	
	private void SetTeamInfoOnClient() {
		gameBase.GetComponent<BaseTeamChoserClient>().SetTeam(ref teamSelect);
	}
}
