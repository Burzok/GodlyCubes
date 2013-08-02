using UnityEngine;
using System.Collections;

public class TowerReloaderServer : MonoBehaviour {
	public Transform bullet;
	
	public Transform bulletPrefabClient;
	public Transform bulletPrefabServer;
	public bool isReloading;
	public float reloadTime = 3;
	
	private Transform detector;
	private Transform spawner;
	private float timer;
	private Team towerTeam;
	private NetworkViewID bulletID;

	void Awake() {
		isReloading = false;
		timer = 0;
		bullet = null;
		detector = transform.Find("Detection");
		spawner = transform.Find("Spawner");
	}
	
	public void SetTeam(Team team) {
		towerTeam = team;
	}
	
	void FixedUpdate() {
		CheckForReaload();
		IncrementTimer();
		RealoadIfItsTime();
	}
	
	private void CheckForReaload() {
		if(bullet == null) {
			isReloading = true;
		}
	}
	
	private void IncrementTimer() {
		if (isReloading)
			timer += Time.deltaTime;
	}
	
	private void RealoadIfItsTime() {
		if (isReloading && timer >= reloadTime)
			Reload();
	}
	
	private void Reload() {
		AllocateBulletID();
		networkView.RPC("InstantiateTowerBullet", RPCMode.All, bulletID);
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
		
		timer = 0;
		isReloading = false;
	}
}
