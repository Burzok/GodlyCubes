using UnityEngine;
using System.Collections;

public class ControllerBasic : MonoBehaviour {
	
	public float rotateSpeed = 250f;
	public float rotationInput;
	
	private GameObject[] mainCam;
	private PlayerGameData data;
	private bool resFlag = false;
	private float Timer=0;
	
	void Awake() {
		mainCam = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		
		data = gameObject.GetComponent<PlayerGameData>();
		
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
			//rotationInput = Input.GetAxis("Mouse X");
			if ( rotationInput != 0 )
				transform.Rotate( transform.up, rotateSpeed * rotationInput * Time.deltaTime);
		}
	}
	
	void Update() {
		if(networkView.isMine)
			rotationInput = Input.GetAxis("Mouse X");
	
		if(resFlag)
			Respawn();
	}
	
	void Respawn() {
		Timer += Time.deltaTime;
		if(Timer >= data.respawnTime ) {
			networkView.RPC("ChangePlayerState",RPCMode.AllBuffered, networkView.viewID);
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
	
			Timer = 0;
			resFlag = false;
		}
	}
	
	void Hit(int damage) {
		data.health -= damage;
		if(data.health <= 0){
			//networkView.RPC("TurnDeadClientCameras",RPCMode.Others, networkView.viewID);
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
			resFlag = true;
			//Network.Destroy(this.gameObject);
		}
		else
			networkView.RPC("SendHitConfirmationToClients", RPCMode.OthersBuffered, networkView.viewID, damage);
	}
	
	[RPC]
	void SendHitConfirmationToClients(NetworkViewID viewID, int damage) {
		if(networkView.viewID == viewID)
			data.health -= damage;
	}
	
	[RPC]
	void TurnDeadClientCameras(NetworkViewID viewID) {
		if(networkView.viewID == viewID)
			TurnONMainCameras();
	}
	
	[RPC]
	void SwichPlayerState(NetworkViewID viewID) {
		if(networkView.viewID == viewID) {
			this.rigidbody.useGravity = !this.rigidbody.useGravity;
			this.collider.enabled = !this.collider.enabled;
			this.GetComponent<MovementBasic>().enabled = !this.GetComponent<MovementBasic>().enabled;
			this.GetComponent<Shooting>().enabled = !this.GetComponent<Shooting>().enabled;
			this.transform.FindChild("Mesh").renderer.enabled = !this.transform.FindChild("Mesh").renderer.enabled;
		}
	}
	
	[RPC]
	void ChangePlayerState(NetworkViewID viewID) {
		if(networkView.viewID == viewID) {
			this.transform.position = data.respawnPosition;
			data.health = data.maxHealth;
		}
	}
}
