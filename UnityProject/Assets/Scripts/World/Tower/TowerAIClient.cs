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
	private void ShootOnClient(NetworkViewID targetID) {
		FindTowerTarget(ref targetID);
		SetTargetOnBullet();
	}
	
	private void FindTowerTarget(ref NetworkViewID targetID) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) 
			if(player.networkView.viewID == targetID)
				target = player.transform;
	}
	
	private void SetTargetOnBullet() {
		towerReloader.bullet.GetComponent<TowerBulletClient>().SetTarget(target);
		//towerReloader.bullet = null;
	}
}
