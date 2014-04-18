using UnityEngine;
using System.Collections;

public class ObstacleLifeClient : MonoBehaviour {

	public GameObject collapseCube;
	public int amountOfCubes = 30;
	public int amountOfParticles = 100;

	[RPC]
	void DestroyObstacle(NetworkViewID obstacleID){
		Debug.Log ("ReceivedID: " + obstacleID);
		Debug.Log ("LocalID: " + networkView.viewID);
		if(obstacleID == networkView.viewID) {
			particleSystem.Emit(amountOfParticles);
			for(int i = 0; i < amountOfCubes; i++) {
				GameObject cube = Instantiate(collapseCube,transform.position, Random.rotation) as GameObject;
				cube.rigidbody.AddForce(Vector3.up*50);
			}
			gameObject.collider.enabled = false;
			gameObject.renderer.enabled = false;
			Destroy(this.gameObject,2f);}
	}
}
