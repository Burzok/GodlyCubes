using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	public Transform bulletPrefab;
	
	private Transform bulletSpawner;
	
	void Awake() {
		bulletSpawner = this.transform.FindChild("BulletSpawner");
	}
	
	void Update() {
		if(networkView.isMine) {
			if(Input.GetMouseButtonDown(0))
				networkView.RPC("SpawnBullet", RPCMode.Server, networkView.viewID);
		}
	}
	
	[RPC]
	void SpawnBullet(NetworkViewID owner) {
		Transform bullet = (Transform)Network.Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.rotation,0);
		bullet.GetComponent<BulletController>().owner=owner;
	}
	
}
