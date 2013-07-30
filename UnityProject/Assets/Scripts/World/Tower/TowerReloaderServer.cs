using UnityEngine;
using System.Collections;

public class TowerReloaderServer : MonoBehaviour {
	public Transform bullet;
	
	public Transform bulletPrefabClient;
	public Transform bulletPrefabServer;
	
	public float reloadTime = 3;
	
	private Transform detector;
	private Transform spawner;
	private float timer;
	private Team towerTeam;
	private NetworkViewID bulletID;
	
	void Awake() {
		timer = 0;
		bullet = null;
		detector = transform.FindChild("Detection");
		spawner = transform.FindChild("Spawner");
	}
	
	void FixedUpdate() {
		IncrementTimer();
		RealoadIfItsTime();
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
		AllocateBulletID();
		networkView.RPC("InstantiateTowerBullet", RPCMode.All, bulletID);
		
		timer = 0;
	}
	
	private void AllocateBulletID() {
		bulletID = Network.AllocateViewID();
	}
	
	[RPC]
	private void InstantiateTowerBullet(NetworkViewID id) {
		bullet = Instantiate(bulletPrefabServer, spawner.position, spawner.rotation) as Transform;
		bullet.networkView.viewID = id;
		
		if (towerTeam == Team.TeamA)
			bullet.gameObject.layer = 11;
		else if (towerTeam == Team.TeamB)
			bullet.gameObject.layer = 12;
				
		bullet.GetComponent<TowerBulletServer>().SetReloader(transform.GetComponent<TowerReloaderServer>());
	}
	
	public void SetTeam(Team team) {
		towerTeam = team;
	}
}
