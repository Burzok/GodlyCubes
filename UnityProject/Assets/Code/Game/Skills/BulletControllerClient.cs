using UnityEngine;
using System.Collections;

public class BulletControllerClient : MonoBehaviour {
	public float bulletSpeed = 50f;
	public int amountOfParticles = 10;

	private Transform childBullet;


	void Start() {
		Network.RemoveRPCs(networkView.viewID);
		childBullet = transform.GetChild(0);
	}	

	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
	}

    [RPC]
    void DestroyBullet() {
        Network.RemoveRPCs(networkView.viewID);
		particleSystem.startColor = GetComponentInChildren<Renderer>().material.color;
		particleSystem.Emit(amountOfParticles);
		this.renderer.enabled = false;
		childBullet.renderer.enabled = false;
        Destroy(this.gameObject,2f);
    }
}
