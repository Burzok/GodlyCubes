using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ShootingAnimationControl();

public class Shooting : MonoBehaviour {
	public Transform bulletPrefab;
	public float coolDown = 1.2f;
	public float shootTime = 0.8f;
	public float activationTime = 0.3f;
	List<PlayerData> list;
	
	private Transform bulletSpawner;
	private float coolDownTimer;
	private Animator animator;
	public MeshRenderer bulletRenderer;
	private ShootingAnimationControl shootingAnimationControl;
	
	void Awake() {
		list = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>().playerList;
		bulletSpawner = this.transform.FindChild("BulletSpawner");
		coolDownTimer = 5f;
		
		animator = this.transform.FindChild("Animator").GetComponent<Animator>();
		bulletRenderer = transform.Find("Animator").Find("Armature").Find("Bone").Find("Bone_001").Find("Bone_L").Find("Bone_L_001").Find("Bullet").GetComponent<MeshRenderer>();
		
		shootingAnimationControl = Dumy;
	}
	
	void Update() {
		if(networkView.isMine) {
			coolDownTimer += Time.deltaTime;
			
			if(Input.GetMouseButton(0))
				CheckCoolDown();
			
			shootingAnimationControl();
		}
	}
	
	private void CheckCoolDown() {
		if (coolDownTimer >= coolDown) {
			Debug.Log("111111111111111111111");
			animator.SetBool("Atack", true);
			shootingAnimationControl = BulletActivationControl;
		}
	}
	
	private void BulletActivationControl() {
		Debug.Log("222222222222222222");
		AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
 		float playbackTime = currentState.normalizedTime % 1;
		Debug.Log(playbackTime);
		
		if (playbackTime >= activationTime) {
			bulletRenderer.enabled = true;
			shootingAnimationControl = BulletShootingControl;
		}
	}
	
	private void BulletShootingControl() {
		Debug.Log("3333333333333333");
		AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
 		float playbackTime = currentState.normalizedTime % 1;
		Debug.Log(playbackTime);
		
		if (playbackTime >= shootTime) {
			Debug.Log("SSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
			bulletRenderer.enabled = false;
			Shoot();
			animator.SetBool("Atack", false);
			coolDownTimer = 0f;
			shootingAnimationControl = Dumy;
		}
		
	}
	
	private void Shoot() {
		networkView.RPC("SpawnBullet", RPCMode.Server, networkView.viewID);
	}
	
	[RPC]
	void SpawnBullet(NetworkViewID owner) {
		Transform bullet = (Transform)Network.Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.rotation,0);
		bullet.GetComponent<BulletController>().owner=owner;
		
		
		foreach(PlayerData data in list) {
			if (data.id == owner) {
				if (data.team == Team.TeamA)
					bullet.gameObject.layer = 11;
				else if (data.team == Team.TeamB)
					bullet.gameObject.layer = 12;					
			}
		}
		
	}
	
	private void Dumy() {
		Debug.Log("dumydumy dumy duymgfdsngkjdfsngkjdfg");
	}
	
}
