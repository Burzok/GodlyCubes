using UnityEngine;
using System.Collections;

public class BulletControllerClient : MonoBehaviour {
	public float bulletSpeed = 10f;
	
	void Start() {
		Network.RemoveRPCs(networkView.viewID);		
	}	

	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
	}

    [RPC]
    void DestroyBullet() {
        Network.RemoveRPCs(networkView.viewID);
        Destroy(this.gameObject);
    }
}
