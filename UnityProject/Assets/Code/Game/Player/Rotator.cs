using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float rotateSpeed = 250f;
	
	private float rotationInput = 0f;
	
	void FixedUpdate() {
		if(networkView.isMine == true) {
			rotationInput = Input.GetAxis("Mouse X");	
	
			if ( rotationInput != 0 )
				transform.Rotate( Vector3.up, rotateSpeed * rotationInput * Time.deltaTime);
		}
	}
}
