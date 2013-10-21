using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
	public float smooth = 12f;
	public Transform standardPos;
	
	void Start() {
		standardPos = GameObject.Find ("CamPos").transform;
	}
	
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.deltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.deltaTime * smooth);
	}
}
