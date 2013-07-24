using UnityEngine;
using System.Collections;

public class SpawnBaseClient : MonoBehaviour {

	public Transform basePrefabClient;
	public Team teamSelect;
	public Transform gameBase;
	
	[RPC]
	private void InstantiateBaseOnClient(NetworkViewID baseID) {
		gameBase = Instantiate(basePrefabClient, transform.position, transform.rotation) as Transform;
		gameBase.networkView.viewID = baseID;
		
		SetTeamInfoOnClient(teamSelect);
	}
	
	private void SetTeamInfoOnClient(Team team) {
		//gameBase.GetComponent<BaseTeamChoserClient>().SetTeam(teamSelect);
	}
}
