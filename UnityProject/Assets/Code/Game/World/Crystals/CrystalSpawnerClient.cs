using UnityEngine;
using System.Collections;

public class CrystalSpawnerClient : MonoBehaviour {
	public GameObject crystal;
	public GameObject[] crystalsType;
	
	private NetworkViewID crystalID;
	private int crystalTypeIndex;		
	
	[RPC]
	private void InstantiateCrystalOnClient(NetworkViewID crystalID, int aquiredCrystalTypeIndex) {
		crystal = Instantiate(crystalsType[aquiredCrystalTypeIndex], transform.position, transform.rotation) as GameObject;
		crystal.networkView.viewID = crystalID;
	}
}