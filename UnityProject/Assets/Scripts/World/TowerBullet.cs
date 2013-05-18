using UnityEngine;
using System.Collections;

public class TowerBullet : MonoBehaviour {
	public Transform target;
	public float bulletSpeed;
	public int damage = 30;
	public Transform towerDetector;
	
	private bool flag;
	public Transform reloder;
	
	public float modif;
	public float mag;
	
	void Start () {
		target = null;
		flag = true;
		modif = 1f;
		bulletSpeed = 1f;
	}
	
	void FixedUpdate() {
		if (target != null) {
			if (flag) {
				animation.Stop();
				reloder.GetComponent<Reloader>().bullet = null;
				flag = false;
			}
			if (!flag) {
				Vector3 toPos = target.transform.position;
				
				Vector3 vecLenght = new Vector3 (transform.position.x + toPos.x, 
													transform.position.y + toPos.y,
													transform.position.z + toPos.z);
				
				mag = vecLenght.magnitude;
				
				if ((vecLenght.magnitude) <= 45f) {
					modif = 8f;
				}
				else if ((vecLenght.magnitude) <= 50f) {
					modif = 2f;	
				}
				else if ((vecLenght.magnitude) <= 60f) {
					modif = 2f;	
				}
				
				transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * bulletSpeed * modif);
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
