using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerAIServer : MonoBehaviour {
	public Transform target;
	
	private GameObject globalScriptObject;
	public TowerReloaderServer towerReloader;
	
	void Awake() {
		globalScriptObject = GameObject.FindGameObjectWithTag(Tags.gameController);	
		towerReloader = transform.parent.GetComponent<TowerReloaderServer>();
	}
	
	void FixedUpdate() {
		CheckIfTargetIsAlive();
		ShootIfUCan();
	}
	
	private void CheckIfTargetIsAlive() {
		if (target != null) 
			if (!target.GetComponent<PlayerData>().isAlive)
				target = null;
	}
	
	private void ShootIfUCan() {
		if (target != null ) 
			if(!towerReloader.isReloading) 
				if(towerReloader.bullet != null) 
					if (target.GetComponent<PlayerData>().isAlive) 
						Shoot();
	}
		
	private void Shoot() {
		networkView.RPC("ShootOnClient", RPCMode.Others, target.networkView.viewID);
		ShootOnServer();
	}
	
	[RPC]
	private void ShootOnClient(NetworkViewID targetID) 
	{}
	
	private void ShootOnServer() {
		SetTargetOnBullet();
	}

	private void SetTargetOnBullet() {
		SetBulletData();
		ClearBulletReloaderReferenceToReload();
	}
	
	private void SetBulletData() {
		TowerBulletServer towerBulletScript = towerReloader.bullet.GetComponent<TowerBulletServer>();
		towerBulletScript.SetTarget(target);
		towerBulletScript.SetReloader(towerReloader);
	}
	
	private void ClearBulletReloaderReferenceToReload() {
		if(!towerReloader.isReloading)
			towerReloader.bullet = null;
			//towerReloader.isReloading = true;
	}
	
	// by wprowadzic opoznienie wejscia w treager dopiero na Stay przekazuje target
	/*
	void OnTriggerEnter(Collider enemy) {
		if (target == null) 
			target = enemy.transform;
	}
	*/
	void OnTriggerStay(Collider enemy) {
		if (target == null)
			target = enemy.transform;
	}
	
	void OnTriggerExit(Collider enemy) {
		if (target == enemy.transform)
			target = null;
	}
}
