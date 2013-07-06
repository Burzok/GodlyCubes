using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ShootingAnimationControl();

public class Shooting : MonoBehaviour {
	public Transform bulletPrefab;
	public float shootTime = 0.65f;
	public float activationTime = 0.5f;
	public float animSpeed = 2f;
	
	public PlayerData dataPublic;
	
	private List<PlayerData> list;
	private Transform bulletSpawner;
	
	private Animator animator;
	private AnimatorStateInfo currentBaseState;
	
	private MeshRenderer bulletRenderer;
	private ShootingAnimationControl shootingAnimationControl;
	
	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int basicAtackState = Animator.StringToHash("Base Layer.Basic_Atack");	
	
	void Awake() {
		list = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>().playerList;
		bulletSpawner = this.transform.FindChild("BulletSpawner");
		
		animator = this.transform.FindChild("Animator").GetComponent<Animator>();
		bulletRenderer = transform.Find("Animator").Find("Armature").Find("Bone").Find("Bone_001").Find("Bone_L").Find("Bone_L_001").Find("Bullet").GetComponent<MeshRenderer>();
		
		shootingAnimationControl = Dumy;
	}
	
	void Update() {
		if(networkView.isMine) {
			currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
			animator.speed = animSpeed;
			
			if(Input.GetMouseButton(0) && currentBaseState.nameHash == idleState)
				BasicAtack();

			shootingAnimationControl();
		}
	}
	
	private void BasicAtack() {
		animator.SetBool("Atack", true);
		shootingAnimationControl = BulletActivationControl;
	}
	
	private void BulletActivationControl() {
		if (currentBaseState.nameHash == basicAtackState) {
	 		float playbackTime = currentBaseState.normalizedTime % 1;
			
			if (playbackTime >= activationTime) {
				bulletRenderer.enabled = true;
				shootingAnimationControl = BulletShootingControl;
			}
		}
	}
	
	private void BulletShootingControl() {
		if (currentBaseState.nameHash == basicAtackState) {
	 		float playbackTime = currentBaseState.normalizedTime % 1;
			
			if (playbackTime >= shootTime) {
				bulletRenderer.enabled = false;
				Shoot();
				animator.SetBool("Atack", false);
				shootingAnimationControl = Dumy;
			}
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
				
				dataPublic = data;
				bullet.transform.GetChild(0).renderer.material.color = new Color(data.color.x, data.color.y, data.color.z);
			}
		}
	}
	
	private void Dumy() {
	}
	
}
