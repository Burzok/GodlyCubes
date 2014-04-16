using UnityEngine;
using System.Collections;

public class ObstacleSpawnerClient : MonoBehaviour {

	public Transform obstaclePrefabClient;
	public Transform gameObstacle;

	[RPC]
	private void InstantiateObstacleOnClient(NetworkViewID obstacleID, Vector3 scale, Quaternion rotation) 
	{
		gameObstacle = Instantiate(obstaclePrefabClient, transform.position, rotation) as Transform;
		gameObstacle.localScale = scale;

		gameObstacle.networkView.viewID = obstacleID;
	}
}
