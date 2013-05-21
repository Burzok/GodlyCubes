using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	public Transform bulletPrefab;
	public float coolDown = 1.2f;
	
	private Transform bulletSpawner;
	private float coolDownTimer;
	
	void Awake() {
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
	}
	
}
