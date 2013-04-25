using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	public float bulletSpeed = 10f;
	public int demage = 10;
	
	void Start() {
		Destroy(this.gameObject,2);
	}
	
	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			other.SendMessage("Hit",demage);
			Network.Destroy(this.gameObject);
		}
	}
}
