using UnityEngine;
using System.Collections;

public class TowerReloaderClient : MonoBehaviour {
	public Transform bullet;
	public Transform bulletPrefabClient;
	public Transform spawner;
	
	void Awake() {
		bullet = null;
		spawner = transform.FindChild("Spawner");
	}
		
	[RPC]
	private void InstantiateTowerBullet(NetworkViewID id) {
		bullet = Instantiate(bulletPrefabClient, spawner.position, spawner.rotation) as Transform;
		bullet.networkView.viewID = id;
	}
}
