using UnityEngine;
using System.Collections;

public class SpawnerCube : MonoBehaviour {

	 public Transform cubePrefab;
	
    void OnGUI() {
        if (GUILayout.Button("SpawnBox")) {
            NetworkViewID viewID = Network.AllocateViewID();
            networkView.RPC("SpawnBox", RPCMode.AllBuffered, viewID, transform.position);
        }
    }
	
    [RPC]
    void SpawnBox(NetworkViewID viewID, Vector3 location) {
        Transform clone;
        clone = Instantiate(cubePrefab, location, Quaternion.identity) as Transform as Transform;
        NetworkView nView;
        nView = clone.GetComponent<NetworkView>();
        nView.viewID = viewID;
    }
}
