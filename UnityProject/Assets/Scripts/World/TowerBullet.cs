using UnityEngine;
using System.Collections;

public class TowerBullet : MonoBehaviour {
	public Transform target;
	public float bulletSpeed = 1f;
	public int damage = 30;
	public Transform towerDetector;
	
	private bool flag;
	
	void Start () {
		target = null;
		flag = true;
	}
	
	void FixedUpdate() {
		if (target != null) {
			if (flag) {
				animation.Stop();

				flag = false;
			}
			if (!flag) {
				Vector3 toPos = target.transform.position;
				transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * bulletSpeed);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			other.SendMessage("Hit", damage);
			Network.RemoveRPCs(networkView.viewID);
			Network.Destroy(this.gameObject);
		}
	}
}
