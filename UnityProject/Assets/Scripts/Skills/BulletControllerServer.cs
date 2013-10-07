using UnityEngine;
using System.Collections;

public class BulletControllerServer : MonoBehaviour {
	public float bulletSpeed = 10f;
	public float lifeTime = 2.0f;
	public int damage = 10;
	public NetworkViewID owner;
	
	void Start() {
		if(Network.isServer)
			StartCoroutine(DestroyAfterLifeTime());
	}
	
	IEnumerator DestroyAfterLifeTime() {
		yield return new WaitForSeconds(lifeTime);
        Network.RemoveRPCs(networkView.viewID);
        networkView.RPC("DestroyBullet", RPCMode.Others);
        Destroy(this.gameObject);
	}
	
	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			object[] data = new object[2];
			data[0]=damage;
			data[1]=owner;
			other.SendMessage("Hit", data, SendMessageOptions.DontRequireReceiver);
            Network.RemoveRPCs(networkView.viewID);
            networkView.RPC("DestroyBullet", RPCMode.Others);
            Destroy(this.gameObject);
		}
	}

    [RPC]
    void DestroyBullet() {
        Network.RemoveRPCs(networkView.viewID);
        Destroy(this.gameObject);
    }
}
