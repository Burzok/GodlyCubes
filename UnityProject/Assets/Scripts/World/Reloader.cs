using UnityEngine;
using System.Collections;

public class Reloader : MonoBehaviour {
	public Transform bullet;
	
	public Transform bulletPrefabClient;
	public Transform bulletPrefabServer;
	
	public float reloadTime = 3;
	public Transform target;
	
	private Transform detector;
	private Transform spawner;
	private float timer;
	private Team towerTeam;
	
	void Awake() {
		timer = 0;
		bullet = null;
		target = null;
		detector = transform.FindChild("Detection");
		spawner = transform.FindChild("Spawner");
		if(Network.isServer)
			networkView.RPC("FirstReload", RPCMode.All);
	}
	
	void FixedUpdate() {
		if (Network.isServer) {
			CheckIfTargetIsAlive();
			IncrementTimer();
			RealoadIfItsTime();
		}
	}
	/*
	void Update() {
		if(Input.GetKeyDown(KeyCode.A)) {
			Network.Destroy(bullet.gameObject);	
		}	
		if(Input.GetKeyDown(KeyCode.D)) {
			Network.Destroy(this.gameObject);	
		}	
	}
	*/
	private void CheckIfTargetIsAlive() {
		if (target != null) {
			if (!target.GetComponent<PlayerData>().isAlive)
				target = null;
		}
	}
	
	private void IncrementTimer() {
		if (bullet == null)
			timer += Time.deltaTime;
	}
	
	private void RealoadIfItsTime() {
		if (bullet == null && timer >= reloadTime)
			Reload();
	}
	
	private void Reload() {
		NetworkViewID viewID = Network.AllocateViewID();
		
		networkView.RPC("InstantiateTowerBullet", RPCMode.All, viewID);
		
		timer = 0;
	}
	
	[RPC]
	private void FirstReload() {
		NetworkViewID viewID = Network.AllocateViewID();
		
		networkView.RPC("InstantiateTowerBullet", RPCMode.All, viewID);
		
		timer = 0;
	}
	
	
	[RPC]
	private void InstantiateTowerBullet(NetworkViewID id) {
		if(Network.isServer) {
			bullet = Instantiate(bulletPrefabServer, spawner.position, spawner.rotation) as Transform;
			bullet.networkView.viewID = id;
			
			if (towerTeam == Team.TeamA)
				bullet.gameObject.layer = 11;
			else if (towerTeam == Team.TeamB)
				bullet.gameObject.layer = 12;
						
			bullet.GetComponent<TowerBulletServer>().towerDetector = detector;
			bullet.GetComponent<TowerBulletServer>().reloder = this.transform;
			
			detector.GetComponent<TowerAIServer>().bullet = bullet;
		}
		else {
			bullet = Instantiate(bulletPrefabClient, spawner.position, spawner.rotation) as Transform;
			bullet.networkView.viewID = id;
		}
	}
	
	public void SetTeam(Team team) {
		towerTeam = team;
	}
}
