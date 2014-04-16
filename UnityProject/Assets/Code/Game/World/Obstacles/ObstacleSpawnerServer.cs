using UnityEngine;
using System.Collections;

public class ObstacleSpawnerServer : MonoBehaviour {

	public Transform obstaclePrefabServer;

	public float scaleMin = 1;
	public float scaleMax = 6;
	private Transform gameObstacle;
	private NetworkViewID obstacleID;

	void Awake() {
		networkView.group = 0;
		AllocateNetworkID();	

		InstantiateObstacleOnServer();
		RandomizeAndSetSizeAndRotation();
	}

	private void AllocateNetworkID() {
		obstacleID = Network.AllocateViewID();	
	}

	private void InstantiateObstacleOnServer() {
		gameObstacle = Instantiate(obstaclePrefabServer, transform.position, transform.rotation) as Transform;
		gameObstacle.networkView.viewID = obstacleID;
	}

	private void RandomizeAndSetSizeAndRotation() { 
		Vector3 scale = new Vector3(Random.Range(scaleMin,scaleMax),Random.Range(scaleMin,scaleMax),Random.Range(scaleMin,scaleMax));
		Quaternion rotation = Random.rotation;

		gameObstacle.localScale = scale;
		gameObstacle.rotation = rotation;
	}

	public void SpawnObstacleOnClient(NetworkPlayer sender) {
		networkView.RPC("InstantiateObstacleOnClient", sender, obstacleID, gameObstacle.localScale, gameObstacle.rotation);
	}
	
	[RPC]
	private void InstantiateObstacleOnClient(NetworkViewID obstacleID, Vector3 scale, Quaternion rotation) 
	{}
}
