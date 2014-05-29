using UnityEngine;
using System.Collections;

public class TowerBulletServer : MonoBehaviour 
{
	public Transform target;
	public float bulletSpeed;
	public int damage = 30;
	public NetworkViewID owner;
	
	public bool flag;
	public TowerReloaderServer reloder;
		
	public PlayerData targetData;
	
	void Awake() 
	{
		target = null;
		owner = networkView.viewID;
		flag = true;
		bulletSpeed = 40f;
	}
	
	public void SetTarget(Transform newTarget) 
	{
		target = newTarget;
		targetData = target.GetComponent<PlayerData>();
	}
	
	public void SetReloader(TowerReloaderServer newReloader) 
	{
		reloder = newReloader;
	}
	
	void FixedUpdate() 
	{
		BulletMovement();
	}
	
	private void BulletMovement() 
	{
		if (target != null) 
		{
			if (flag) 
			{
				flag = false;
			}
			else 
			{
				transform.LookAt(target);
				transform.Translate( 0, 0, Time.deltaTime * bulletSpeed);
			}
		}
	}
	
	void Update() 
	{
		DestroyIfTargetIsDead();
	}
	
	private void DestroyIfTargetIsDead() 
	{
		if(target != null && !targetData.isAlive)
		{
			Network.Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		SendHitData( ref other);
		DestroyBullet();
	}
	
	private void SendHitData(ref Collider other) 
	{
		object[] data = new object[2];
		data[0] = damage;
		data[1] = owner;
		other.SendMessage( "Hit", data);
	}
	
	private void DestroyBullet() 
	{
		Network.RemoveRPCs( networkView.viewID);
		Network.Destroy( this.gameObject);
	}
}
