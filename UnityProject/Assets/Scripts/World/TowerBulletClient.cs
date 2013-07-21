using UnityEngine;
using System.Collections;

public class TowerBulletClient : MonoBehaviour {
	public Transform target;
	public float bulletSpeed;
	public int damage = 30;
	public Transform towerDetector;
	public NetworkViewID owner;
	
	private bool flag;
	public Transform reloder;
	
	public float modif;
	public float mag;
	
	void Start () {
		owner = networkView.viewID;
		target = null;
		flag = true;
		modif = 4f;
		bulletSpeed = 1f;
	}
	
	void FixedUpdate() {
		if (target != null) {
			if (flag) {
				animation.Stop();
				animation.CrossFade("TowerBulletShoot");
				reloder.GetComponent<Reloader>().bullet = null;
				flag = false;
			}
			if (!flag) {
				Vector3 toPos = target.transform.position;
				
				Vector3 vecLenght = new Vector3 (transform.position.x + toPos.x, 
													transform.position.y + toPos.y,
													transform.position.z + toPos.z);
				
				mag = vecLenght.magnitude;
				
				if ((vecLenght.magnitude) <= 70f) {
					modif = 8f;
				}
				else if ((vecLenght.magnitude) <= 100f) {
					modif = 6f;	
				}
				
				transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * bulletSpeed * modif);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			object[] data = new object[2];
			data[0]=damage;
			data[1]=owner;
			other.SendMessage("Hit", data);
			Network.RemoveRPCs(networkView.viewID);
			Network.Destroy(this.gameObject);
		}
	}
}
