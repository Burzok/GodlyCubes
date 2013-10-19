using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ShootingAnimationControl();

public class ShootingControllerClient : MonoBehaviour {
	public Transform bulletPrefabClient;
	
	public float shootTime = 0.65f;
	public float activationTime = 0.5f;
	public float animSpeed = 2f;
	
	private PlayerData data;

    private Transform bullet;
	private Transform bulletSpawner;
	
	private Animator animator;
	private AnimatorStateInfo currentBaseState;
	
	private MeshRenderer bulletRenderer;
	private ShootingAnimationControl shootingAnimationControl;
	
	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int basicAtackState = Animator.StringToHash("Base Layer.Basic_Atack");	
	
	void Awake() {
		networkView.group = 0;
		
		bulletSpawner = this.transform.FindChild("BulletSpawner");
		data = GetComponent<PlayerData>();
		
        animator = this.transform.FindChild("Animator").GetComponent<Animator>();
		bulletRenderer = transform.Find("Animator").Find("Armature").Find("Bone").Find("Bone_001").Find("Bone_L")
			.Find("Bone_L_001").Find("Bullet").GetComponent<MeshRenderer>();
		
		shootingAnimationControl = Idle;
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
				shootingAnimationControl = Idle;
			}
		}
	}
	
	private void Shoot() {
		networkView.RPC("ServerSpawnBulletCall", RPCMode.Server, networkView.viewID);
	}
		
	[RPC]
	void SpawnBulletOnClient(NetworkViewID owner, NetworkViewID bulletID) {      
		bullet = Instantiate(
		    bulletPrefabClient, bulletSpawner.transform.position, bulletSpawner.rotation) as Transform;
       
        bullet.networkView.viewID = bulletID;

        SetLayers();
        SetColor();
	}

    [RPC]
    void ServerSpawnBulletCall(NetworkViewID owner) {}

    private void SetLayers() {
        if (data.team == Team.TEAM_A)
            bullet.gameObject.layer = 11;
        else if (data.team == Team.TEAM_B)
            bullet.gameObject.layer = 12;
    }

    private void SetColor() {
        bullet.GetChild(0).renderer.material.color = new Color(data.color.x, data.color.y, data.color.z);
    }
	
	private void Idle() {
	}
	
}
