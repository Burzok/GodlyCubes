using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAIClient : MonoBehaviour {
	public Transform target;
	
	private GameObject globalScriptObject;
	private TowerReloaderClient towerReloader;
	private LineRenderer laserPointer;
	
	void Awake() {
		globalScriptObject = GameObject.FindGameObjectWithTag(Tags.gameController);	
		towerReloader = transform.parent.GetComponent<TowerReloaderClient>();
		laserPointer = transform.parent.Find("Laser").GetComponent<LineRenderer>();
		laserPointer.SetPosition(0,laserPointer.transform.position);
		laserPointer.SetPosition(1,laserPointer.transform.position);
	}
	
	void FixedUpdate() {
		SetLaserPointerIfTargetIsNotNull();
	}
	
	private void SetLaserPointerIfTargetIsNotNull() {
		if(target != null)
			laserPointer.SetPosition(1,target.transform.position);
		else
			laserPointer.SetPosition(1,laserPointer.transform.position);
	}
	
	[RPC]
	private void ResetTarget() {
		target = null;
	}
	
	[RPC]
	private void ShootOnClient(NetworkViewID targetID) {
		FindTowerTarget(ref targetID);
		SetTargetOnBullet();
	}
	
	private void FindTowerTarget(ref NetworkViewID targetID) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) 
			if(player.networkView.viewID == targetID) {
				target = player.transform;
				laserPointer.SetPosition(1,target.transform.position);
			}
	}
	
	private void SetTargetOnBullet() {
		towerReloader.bullet.GetComponent<TowerBulletClient>().SetTarget(target);
		towerReloader.bullet = null;
	}
}
