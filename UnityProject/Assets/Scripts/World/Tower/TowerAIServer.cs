using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAIServer : MonoBehaviour {
	public Transform target;
	
	private GameObject globalScriptObject;
	private TowerReloaderServer towerReloader;
	
	void Awake() {
		globalScriptObject = GameObject.FindGameObjectWithTag(Tags.gameController);	
		towerReloader = transform.parent.GetComponent<TowerReloaderServer>();
	}
	
	void FixedUpdate() {
		CheckIfTargetIsAlive();
		
		if (target != null && towerReloader.bullet != null) {
			if (target.GetComponent<PlayerData>().isAlive) {
				networkView.RPC("ShootOnClient", RPCMode.Others, target.networkView.viewID);
				ShootOnServer();
			}
			else   
				networkView.RPC("ClearTowerTarget", RPCMode.All, networkView.viewID);
		}
	}
	
	private void CheckIfTargetIsAlive() {
		if (target != null) 
			if (!target.GetComponent<PlayerData>().isAlive)
				target = null;
	}
	
	[RPC]
	private void ShootOnClient(NetworkViewID targetID) 
	{}
	
	private void ShootOnServer() {
		SetTargetOnBullet();
	}

	private void SetTargetOnBullet() {
		towerReloader.bullet.GetComponent<TowerBulletServer>().target = target;
		towerReloader.bullet = null;
	}
	
	/* by wprowadzic opoznienie wejscia w treager dopiero na Stay przekazuje target
	void OnTriggerEnter(Collider other) {
		if (target == null) 
			networkView.RPC("SetTowerTarget", RPCMode.All, networkView.viewID, other.networkView.viewID);
	}
	*/
	void OnTriggerStay(Collider other) {
		if (target == null) 
			networkView.RPC("SetTowerTarget", RPCMode.All, networkView.viewID, other.networkView.viewID);
	}
	
	void OnTriggerExit(Collider other) {
			if (target == other.transform)
				networkView.RPC("ClearTowerTarget", RPCMode.All, networkView.viewID);
	}
	
	[RPC]
	private void SetTowerTarget(NetworkViewID viewID, NetworkViewID targetID) { //TODO: split into functions
		if(networkView.viewID == viewID) {
			List<PlayerData> playerDataList = globalScriptObject.GetComponent<PlayerList>().playerList;
			foreach( PlayerData playerData in playerDataList ) 
				if ( targetID == playerData.id ) 
					target = playerData.transform;
		}
	}
	
	[RPC]
	private void ClearTowerTarget(NetworkViewID viewID) {
		if(networkView.viewID == viewID) 
			target = null;
	}
}
