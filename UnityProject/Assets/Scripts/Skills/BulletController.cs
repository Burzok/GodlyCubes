using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	public float bulletSpeed = 10f;
	public int damage = 10;
	public float lifeTime = 2.0f;
	
	void Start() {
		Network.RemoveRPCs(networkView.viewID);
		
		if(Network.isServer)
			StartCoroutine(DestroyAfterLifeTime());
	}
	
	IEnumerator DestroyAfterLifeTime() {
		yield return new WaitForSeconds(lifeTime);
		Network.Destroy(this.gameObject);
	}
	
	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			other.SendMessage("Hit", damage);
			Network.Destroy(this.gameObject);
		}
	}
}
