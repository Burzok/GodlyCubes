using UnityEngine;
using System.Collections;

public class TowerBulletServer : MonoBehaviour {
	public Transform target;
	public float bulletSpeed;
	public int damage = 30;
	public NetworkViewID owner;
	
	private bool flag;
	public TowerReloaderServer reloder;
	
	public float modif;
	public float mag;
	
	private PlayerData targetData;
	
	void Start () {
		owner = networkView.viewID;
		target = null;
		flag = true;
		modif = 4f;
		bulletSpeed = 1f;
	}
	
	public void SetTarget(Transform newTarget) {
		Debug.LogWarning("SETING TARGET ON BULLET");
		target = newTarget;
		targetData = target.GetComponent<PlayerData>();
	}
	
	public void SetReloader(TowerReloaderServer newReloader) {
		reloder = newReloader;
	}
	
	void FixedUpdate() {
		BulletMovement();
	}
	
	private void BulletMovement() { //TODO: this sucks nead a redo
		if (target != null) {
			if (flag) {
				flag = false;
			}
			else {
				Vector3 toPos = target.transform.position;
				Vector3 fromPos = transform.position;
				Vector3 vecLenght = new Vector3(fromPos.x+toPos.x, fromPos.y+toPos.y, fromPos.z+toPos.z);
				
				mag = vecLenght.magnitude;
				
				if (vecLenght.magnitude <= 70f) 
					modif = 8f;
				else if (vecLenght.magnitude <= 100f) 
					modif = 6f;	
				
				transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * bulletSpeed * modif);
			}
		}
	}
	
	void Update() {
		DestroyIfTargetIsDead();
	}
	
	private void DestroyIfTargetIsDead() {
		if(target != null)
			if(!targetData.isAlive)
				Network.Destroy(this.gameObject);
	}
	
	void OnTriggerEnter(Collider other) {
		SendHitData(ref other);
		DestroyBullet();
	}
	
	private void SendHitData(ref Collider other) {
		object[] data = new object[2];
		data[0]=damage;
		data[1]=owner;
		other.SendMessage("Hit", data);
	}
	
	private void DestroyBullet() {
		Network.RemoveRPCs(networkView.viewID); // sprawdzic czy przy braku tego jest jakas roznica
		Network.Destroy(this.gameObject);
	}
}
