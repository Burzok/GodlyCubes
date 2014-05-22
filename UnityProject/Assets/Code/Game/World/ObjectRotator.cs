using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour {
	
	void Update() {
		transform.RotateAround(gameObject.transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
