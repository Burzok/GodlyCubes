using UnityEngine;
using System.Collections;

public class TowerBulletClient : MonoBehaviour {
	public Transform target;
	public float bulletSpeed;
	
	public bool flag;
	
	public float modif;
	public float mag;
	
	void Awake() {
		target = null;
		flag = true;
		modif = 4f;
		bulletSpeed = 1f;
	}
	
	public void SetTarget(Transform newTarget) {
		target = newTarget;
	}
	
	void FixedUpdate() { 
			BulletMovement();
	}
	
	private void BulletMovement() {  //TODO: this sucks nead a redo
		if (target != null) {
			if (flag) {
				animation.Stop();
				animation.CrossFade("TowerBulletShoot");
				flag = false;
			}
			else {
				Vector3 toPos = target.position;
				Vector3 fromPos = transform.position;
				Vector3 vecLenght = new Vector3(fromPos.x+toPos.x, fromPos.y+toPos.y, fromPos.z+toPos.z);
				
				mag = vecLenght.magnitude;
				
				if ((vecLenght.magnitude) <= 70f)
					modif = 8f;
				else if ((vecLenght.magnitude) <= 100f) 
					modif = 6f;
				
				transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * bulletSpeed * modif);
			}
		}
	}
}
