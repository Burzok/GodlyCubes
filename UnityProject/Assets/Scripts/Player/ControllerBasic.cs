using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerBasic : MonoBehaviour {
	
	public float rotateSpeed = 250f;
	public float rotationInput;
	public int damage = 10;
	
	private GameObject[] mainCam;
	private GameObject mainScript;
	private PlayerGameData data;
	private bool resFlag = false;
	private float timer=0;
	private Transform meshChild;
	
	void Awake() {
		mainCam = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		mainScript = GameObject.FindGameObjectWithTag(Tags.gameController);
		
		data = gameObject.GetComponent<PlayerGameData>();
		
		meshChild = this.transform.FindChild("Mesh");
		
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
		timer += Time.deltaTime;
		if(timer >= data.respawnTime ) {
			networkView.RPC("ChangePlayerState",RPCMode.AllBuffered, networkView.viewID);
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
	
			timer = 0;
			resFlag = false;
		}
	}
	
	void Hit(object []package) {
		data.health -= package[0];
		if(data.health <= 0){
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
			
			List<PlayerData> playerList = mainScript.GetComponent<PlayerList>().playerList;
			int id = playerList.FindIndex(player => player.id == networkView.viewID);
			playerList[id].deaths++;			
			networkView.RPC ("UpdateDeath", RPCMode.Others, networkView.viewID);
			
			id = playerList.FindIndex(player => player.id == package[1]);
			playerList[id].kills++;	
			networkView.RPC ("UpdateKill", RPCMode.Others, package[1]);
			
			resFlag = true;
		}
		else
			networkView.RPC("SendHitConfirmationToClients", RPCMode.OthersBuffered, networkView.viewID, damage);
	}
	
	[RPC]
	void UpdateKill(NetworkViewID viewID) {
		List<PlayerData> playerList = mainScript.GetComponent<PlayerList>().playerList;
		int id = playerList.FindIndex(player => player.id == viewID);
		playerList[id].kills++;		
	}
	
	[RPC]
	void UpdateDeath(NetworkViewID viewID) {
		List<PlayerData> playerList = mainScript.GetComponent<PlayerList>().playerList;
		int id = playerList.FindIndex(player => player.id == viewID);
		playerList[id].deaths++;		
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
			this.GetComponent<PlayerGameData>().isAlive = !this.GetComponent<PlayerGameData>().isAlive;
			this.collider.enabled = !this.collider.enabled;
			this.GetComponent<MovementBasic>().enabled = !this.GetComponent<MovementBasic>().enabled;
			this.GetComponent<Shooting>().enabled = !this.GetComponent<Shooting>().enabled;
			meshChild.renderer.enabled = !meshChild.renderer.enabled;
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