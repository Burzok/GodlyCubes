using UnityEngine;
using System.Collections;

public class CrystalSpawnerServer : MonoBehaviour {
	public GameObject crystal;
	public GameObject[] crystalsType;	
	public float respawnTime = 10f;	
	public float timer;
	
	private NetworkViewID crystalID;	
	private int crystalTypeIndex;		
	
	void Awake () {		
		networkView.group = 0;		
		crystalID = Network.AllocateViewID();
		
		RandomizeSpawningCrystal();
		InstantiateCrystalOnServer();
		timer = 0;
	}
	
	void FixedUpdate() {
		IncrementTimer();
		SpawnIfReady();
	}
		
	private void IncrementTimer() {
		if (crystal == null)
			timer += Time.deltaTime;
	}
	
	private void SpawnIfReady() {
		if (crystal == null && timer >= respawnTime) {
			RandomizeSpawningCrystal();
			crystalID = Network.AllocateViewID();
			InstantiateCrystalOnServer();
			networkView.RPC("InstantiateCrystalOnClient", RPCMode.Others, crystalID, crystalTypeIndex);
			timer = 0f;
		}
	}
	
	private void InstantiateCrystalOnServer() {
		crystal = Instantiate(crystalsType[crystalTypeIndex], transform.position, transform.rotation) as GameObject;
		crystal.networkView.viewID = crystalID;
	}
	
	private void RandomizeSpawningCrystal() {
		int crystalRangeBegin = 0;
		int crystalRangeEnd = crystalsType.Length;
		crystalTypeIndex = Random.Range(crystalRangeBegin, crystalRangeEnd);		
	}
	
	public void SpawnCrystalOnClient(ref NetworkPlayer sender) {
		networkView.RPC("InstantiateCrystalOnClient", sender, crystalID, crystalTypeIndex);
	}
	
	[RPC]
	private void InstantiateCrystalOnClient(NetworkViewID crystalID, int aquiredCrystalTypeIndex) 
	{}
}
