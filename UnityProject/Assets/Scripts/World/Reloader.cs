using UnityEngine;
using System.Collections;

public class Reloader : MonoBehaviour {
	public Transform bullet;
	public Transform bulletPrefab;
	public float reloadTime = 3;
	public Transform target;
	
	private Transform detector;
	private Transform spawner;
	private float timer;
	
	void Awake() {
		timer = 0;
		bullet = null;
		target = null;
		detector = transform.FindChild("Detection");
		spawner = transform.FindChild("Spawner");
	}
	
	void Update() {
<<<<<<< HEAD
		if (bullet == null)
			timer += Time.deltaTime;
		
		if (bullet == null && timer >= reloadTime) {
			bullet = Network.Instantiate(bulletPrefab, spawner.position, spawner.rotation, 0) as Transform;
			timer = 0;
=======
		if (Network.isServer) {
			if (bullet == null)
				timer += Time.deltaTime;
			
			if (bullet == null && timer >= reloadTime) {
				bullet = Network.Instantiate(bulletPrefab, spawner.position, spawner.rotation, 0) as Transform;
				detector.GetComponent<AITower>().bullet = bullet;
				bullet.GetComponent<TowerBullet>().towerDetector = detector;
				timer = 0;
			}
>>>>>>> origin/Basic-Gameplay---4-Towers
		}
	}
}
