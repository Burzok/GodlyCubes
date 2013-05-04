using UnityEngine;
using System.Collections;

public class Reloader : MonoBehaviour {
	public Transform bullet;
	public Transform bulletPrefab;
	public float reloadTime = 3;
	
	private Transform spawner;
	public float timer;
	
	void Awake() {
		timer = 0;
		bullet = null;
		spawner = transform.FindChild("Spawner");
	}
	
	void Update() {
		if (bullet == null)
			timer += Time.deltaTime;
		
		if (bullet == null && timer >= reloadTime) {
			bullet = Network.Instantiate(bulletPrefab, spawner.position, spawner.rotation, 0) as Transform;
			timer = 0;
		}
	}
}
