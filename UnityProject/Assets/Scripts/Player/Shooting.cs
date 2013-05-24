using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting : MonoBehaviour {
	public Transform bulletPrefab;
	public float coolDown = 1.2f;
	List<PlayerData> list;
	
	private Transform bulletSpawner;
	private float coolDownTimer;
	
	void Awake() {
		
		list = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>().playerList;
		bulletSpawner = this.transform.FindChild("BulletSpawner");
		coolDownTimer = 5f;
	}
	
	void Update() {
		if(networkView.isMine) {
			coolDownTimer += Time.deltaTime;
			
			if(Input.GetMouseButton(0))
				CheckCoolDown();
		}
	}
	
	private void CheckCoolDown() {
		if (coolDownTimer >= coolDown) {
			Shoot();
			coolDownTimer = 0f;
		}
	}
	
	private void Shoot() {
		networkView.RPC("SpawnBullet", RPCMode.Server, networkView.viewID);
	}
	
	[RPC]
	void SpawnBullet(NetworkViewID owner) {
		Transform bullet = (Transform)Network.Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.rotation,0);
		bullet.GetComponent<BulletController>().owner=owner;
		
		
		foreach(PlayerData data in list) {
			if (data.id == owner) {
				if (data.team == Team.TeamA)
					bullet.gameObject.layer = 11;
				else if (data.team == Team.TeamB)
					bullet.gameObject.layer = 12;					
			}
		}
		
	}
	
}
