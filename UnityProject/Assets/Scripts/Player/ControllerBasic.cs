using UnityEngine;
using System.Collections;

public class ControllerBasic : MonoBehaviour {
	public float horizontalInput = 0f;
	public float verticalInput = 0f;
	public float rotationInput = 0f;
	public float forwardSpeed = 20f; 
	public float rotateSpeed = 250f;
	
	private GameObject[] mainCam;
	
	void Awake() {
		if(!networkView.isMine) {
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
			
			if(!Network.isServer)
				TurnOffMainCameras();
		}
		else {
			TurnOffMainCameras();
			transform.FindChild("Camera").gameObject.SetActive(true);
		}
	}
	
	void TurnOffMainCameras() {
		mainCam = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		foreach( GameObject cam in mainCam )
			cam.SetActive(false);
	}
	
	void FixedUpdate() {
		if(networkView.isMine) {
			horizontalInput = Input.GetAxis("Horizontal");
			verticalInput = Input.GetAxis("Vertical");
			rotationInput = Input.GetAxis("Mouse X");
			
			if ( verticalInput != 0 ) {
				//transform.Translate( Vector3.forward  * forwardSpeed * verticalInput * Time.deltaTime);
				rigidbody.AddForce( transform.forward * Time.deltaTime * forwardSpeed * verticalInput, ForceMode.VelocityChange);
			}
			
			if ( horizontalInput != 0 ) {
				//transform.Translate( Vector3.right * forwardSpeed * horizontalInput * Time.deltaTime);
				rigidbody.AddForce( transform.right * Time.deltaTime * forwardSpeed * horizontalInput, ForceMode.VelocityChange);
			}
			
			if ( rotationInput != 0 )
				transform.Rotate( transform.up, rotateSpeed * rotationInput * Time.deltaTime);
		}
	}
}
