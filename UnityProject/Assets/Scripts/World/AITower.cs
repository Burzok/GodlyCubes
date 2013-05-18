using UnityEngine;
using System.Collections;

public class AITower : MonoBehaviour {
	public Transform target;
	public Transform bullet;
	
	void FixedUpdate() {
		if (target != null && bullet != null) {
			if (target.GetComponent<PlayerGameData>().isAlive)
				bullet.GetComponent<TowerBullet>().target = target;
			else
				target = null;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (target == null) 
			target = other.transform;
	}
	
	void OnTriggerStay(Collider other) {
		if (target == null) 
			target = other.transform;
	}
	void OnTriggerExit(Collider other) {
		if (target == other.transform)
			target = null;
	}
}
