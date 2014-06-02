using UnityEngine;
using System.Collections;

public class ObstacleLifeServer : MonoBehaviour {

	public int currentHealth;
	private NetworkViewID obstacleID;

	void Start() {
		currentHealth = GameData.OBSTACLE_MAX_HEALTH;
		obstacleID = networkView.viewID;
	}

	void Hit(object[] package) {
		currentHealth -= (int)package[0];

		if(currentHealth <= 0) {
			networkView.RPC("DestroyObstacle", RPCMode.Others, obstacleID);			
			Destroy(this.gameObject);
		}
	}

	[RPC]
	void DestroyObstacle(NetworkViewID obstacleID){}
}
