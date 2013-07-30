using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAIClient : MonoBehaviour {
	public Transform target;
	
	private GameObject globalScriptObject;
	private TowerReloaderClient towerReloader;
	
	void Awake() {
		globalScriptObject = GameObject.FindGameObjectWithTag(Tags.gameController);	
		towerReloader = transform.parent.GetComponent<TowerReloaderClient>();
	}
	
	[RPC]
	private void Shoot(NetworkViewID targetID) {
		FindTarget(ref targetID);
		SetTargetOnBullet();
	}
	
	private void FindTarget(ref NetworkViewID targetID) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) 
			if(player.networkView.viewID == targetID)
				target = player.transform;
	}
	
	private void SetTargetOnBullet() {
		towerReloader.bullet.GetComponent<TowerBulletClient>().target = target;
		towerReloader.bullet = null;
	}
	
	[RPC]
	private void SetTowerTarget(NetworkViewID viewID, NetworkViewID targetID) { //TODO: split into functions
		if(networkView.viewID == viewID) {
			List<PlayerData> playerDataList = globalScriptObject.GetComponent<PlayerList>().playerList;
			foreach( PlayerData playerData in playerDataList ) {
				if ( targetID == playerData.id )
					target = playerData.transform;
			}
		}
	}
	
	[RPC]
	private void ClearTowerTarget(NetworkViewID viewID) {
		if(networkView.viewID == viewID) 
			target = null;
	}
}
