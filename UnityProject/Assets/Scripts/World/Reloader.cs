using UnityEngine;
using System.Collections;

public class Reloader : MonoBehaviour {
	public Transform bullet;
	public Transform bulletPrefab;
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
		if(Network.isServer || Network.isClient)
			networkView.RPC("FirstReload", RPCMode.All);
	}
	
	void FixedUpdate() {
		if (Network.isServer) {
			CheckIfTargetIsAlive();
			IncrementTimer();
			RealoadIfItsTime();
		}
	}
	
	private void CheckIfTargetIsAlive() {
		if (target != null) {
			if (!target.GetComponent<ControllerBasic>().data.isAlive)
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
		bullet = Instantiate(bulletPrefab, spawner.position, spawner.rotation) as Transform;
		
		if (towerTeam == Team.TeamA)
			bullet.gameObject.layer = 11;
		else if (towerTeam == Team.TeamB)
			bullet.gameObject.layer = 12;
						
		bullet.GetComponent<NetworkView>().viewID = id;
		
		detector.GetComponent<AITower>().bullet = bullet;
		
		bullet.GetComponent<TowerBullet>().towerDetector = detector;
		bullet.GetComponent<TowerBullet>().reloder = this.transform;
	}
	
	public void SetTeam(Team team) {
		towerTeam = team;
	}
}
