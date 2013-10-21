using UnityEngine;
using System.Collections;


public class SpawnTowerClient : MonoBehaviour {

	public Transform towerPrefabClient;
	public Team teamSelect;
	public Transform tower;
	
	[RPC]
	private void InstantiateTowerOnClient(NetworkViewID towerID, NetworkViewID towerDetectorID) {
		tower = Instantiate(towerPrefabClient, transform.position, transform.rotation) as Transform;
		tower.networkView.viewID = towerID;
		tower.GetChild(0).networkView.viewID = towerDetectorID;
		
		SetTeamInfoOnClient(ref teamSelect);
	}
	
	private void SetTeamInfoOnClient(ref Team team) {
		tower.GetComponent<TowerTeamChoserClient>().SetTeam(ref teamSelect);
	}
	
	[RPC]
	private void InstantiateTowerBulletOnClient(NetworkViewID bulletID) {
		TowerReloaderClient reloader = tower.GetComponent<TowerReloaderClient>();
		reloader.bullet = Instantiate(
			reloader.bulletPrefabClient, reloader.spawner.position, reloader.spawner.rotation) as Transform;
		reloader.bullet.networkView.viewID = bulletID;
	}
}
