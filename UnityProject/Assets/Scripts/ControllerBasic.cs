using UnityEngine;
using System.Collections;

public class ControllerBasic : MonoBehaviour {
	
	public float horizontalInput = 0f;
	public float verticalInput = 0f;
	public float rotationInput = 0f;
	
	public float forwardSpeed = 5f; 
	public float rotateSpeed = 250f;
	

	void Start () {
	}
	
	void FixedUpdate(){
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		rotationInput = Input.GetAxis("Mouse X");
		
		//rigidbody.AddForce( direction * Time.deltaTime * forwardSpeed * verticalInput, ForceMode.Acceleration);
		if ( verticalInput != 0 )
			transform.Translate( Vector3.forward  * forwardSpeed * verticalInput * Time.deltaTime);
		if ( horizontalInput != 0 )
			transform.Translate( Vector3.right * forwardSpeed * horizontalInput * Time.deltaTime);
		if ( rotationInput != 0 )
			transform.Rotate( Vector3.up, rotateSpeed * rotationInput * Time.deltaTime);
	}

	void Update () {
	}
}
