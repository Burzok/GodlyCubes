using UnityEngine;
using System.Collections;

public class ControllerBasic : MonoBehaviour {
	
	public float forwardSpeed = 20f; 
	public float rotateSpeed = 250f;
	public int health = 100;
	
	private GameObject[] mainCam;
	private float horizontalInput;
	private float verticalInput;
	private float rotationInput;
	
	void Awake() {
		mainCam = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		
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
		foreach( GameObject cam in mainCam )
			cam.SetActive(false);
	}
	
	void TurnONMainCameras() {
		foreach( GameObject cam in mainCam )
			cam.SetActive(true);
	}
	
	void FixedUpdate() {
		if(networkView.isMine) {
			horizontalInput = Input.GetAxis("Horizontal");
			verticalInput = Input.GetAxis("Vertical");
			rotationInput = Input.GetAxis("Mouse X");
			
			if ( verticalInput != 0 ) {
				rigidbody.AddForce( transform.forward * Time.deltaTime * forwardSpeed * verticalInput, ForceMode.VelocityChange);
			}
			
			if ( horizontalInput != 0 ) {
				rigidbody.AddForce( transform.right * Time.deltaTime * forwardSpeed * horizontalInput, ForceMode.VelocityChange);
			}
			
			if ( rotationInput != 0 )
				transform.Rotate( transform.up, rotateSpeed * rotationInput * Time.deltaTime);
		}
	}
	
	void Hit(int demage) {
		health -= demage;
		if(health <= 0){
			networkView.RPC("TurnDeadClientCameras",RPCMode.Others, networkView.viewID);
			Network.Destroy(this.gameObject);
		}
		else
			networkView.RPC("SendHitConfirmationToClients", RPCMode.OthersBuffered, networkView.viewID, demage);
	}
	
	[RPC]
	void SendHitConfirmationToClients(NetworkViewID viewID, int demage) {
		if(networkView.viewID == viewID)
			health -= demage;
	}
	
	[RPC]
	void TurnDeadClientCameras(NetworkViewID viewID) {
		if(networkView.viewID == viewID)
			TurnONMainCameras();
	}
}
