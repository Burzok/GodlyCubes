using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AITower : MonoBehaviour {
	public Transform target;
	public Transform bullet;
	
	private GameObject globalScriptObject;
	
	void Awake() {
		globalScriptObject = GameObject.FindGameObjectWithTag(Tags.gameController);	
	}
	
	void FixedUpdate() {
		if (Network.isServer) {
			if (target != null && bullet != null) {
				if (target.GetComponent<PlayerGameData>().isAlive) {
					networkView.RPC("SetBulletTarget", RPCMode.All);
				}
				else
					networkView.RPC("ClearTowerTarget", RPCMode.All, networkView.viewID);
			}
		}
	}
	
	[RPC]
	private void SetBulletTarget() {
		TowerBullet towerBulletRefference = bullet.GetComponent<TowerBullet>();
			if (towerBulletRefference.target == null)
				towerBulletRefference.target = target;
	}
	
	void OnTriggerEnter(Collider other) {
		if (Network.isServer) {
			if (target == null) 
				networkView.RPC("SetTowerTarget", RPCMode.All, networkView.viewID, other.networkView.viewID);
				//target = other.transform;
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (Network.isServer) {
			if (target == null) 
				networkView.RPC("SetTowerTarget", RPCMode.All, networkView.viewID, other.networkView.viewID);
				//target = other.transform;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (Network.isServer) {
			if (target == other.transform)
				networkView.RPC("ClearTowerTarget", RPCMode.All, networkView.viewID);
		}
	}
	
	[RPC]
	private void SetTowerTarget(NetworkViewID viewID, NetworkViewID targetID) { //TODO: split into functions
		if(networkView.viewID == viewID) {
			List<PlayerData> playerDataList = globalScriptObject.GetComponent<PlayerList>().playerList;
			foreach( PlayerData playerData in playerDataList ) {
				if ( targetID == playerData.id )
					target = playerData.lokalTransform;
			}
		}
	}
	
	[RPC]
	private void ClearTowerTarget(NetworkViewID viewID) {
		if(networkView.viewID == viewID) 
			target = null;
	}
}
