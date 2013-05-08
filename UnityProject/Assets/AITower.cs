using UnityEngine;
using System.Collections;

public class AITower : MonoBehaviour {
	public Transform target;
	public Transform bullet;
	
	void FixedUpdate() {
		if (target != null && bullet != null) {
			bullet.GetComponent<TowerBullet>().target = target;
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
