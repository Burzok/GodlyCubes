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
		if (Network.isServer) {
			CheckIfTargetIsAlive();
			IncrementTimer();
			RealoadIfItsTime();
		}
	}
	
	private void CheckIfTargetIsAlive() {
		if (target != null) {
			if (!target.GetComponent<PlayerGameData>().isAlive)
				target = null;
		}
	}
	
	private void IncrementTimer() {
		if (bullet == null)
			timer += Time.deltaTime;
	}
	
	private void RealoadIfItsTime() {
		if (bullet == null && timer >= reloadTime)
			Reload();
	}
	
	private void Reload() {
		bullet = Network.Instantiate(bulletPrefab, spawner.position, spawner.rotation, 0) as Transform;
		detector.GetComponent<AITower>().bullet = bullet;
		bullet.GetComponent<TowerBullet>().towerDetector = detector;
		bullet.GetComponent<TowerBullet>().reloder = this.transform;
		timer = 0;
	}
}
