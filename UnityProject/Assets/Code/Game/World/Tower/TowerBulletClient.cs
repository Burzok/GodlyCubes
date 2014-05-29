using UnityEngine;
using System.Collections;

public class TowerBulletClient : MonoBehaviour 
{
	public Transform target;
	public float bulletSpeed;
	
	public bool flag;
	
	void Awake() 
	{
		target = null;
		flag = true;
		bulletSpeed = 10f;
	}
	
	public void SetTarget(Transform newTarget) 
	{
		target = newTarget;
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
				animation.Stop();
				animation.CrossFade("TowerBulletShoot");
				flag = false;
			}
			else 
			{
				transform.LookAt(target);
				transform.Translate( 0, 0, Time.deltaTime * bulletSpeed);
			}
		}
	}
}
