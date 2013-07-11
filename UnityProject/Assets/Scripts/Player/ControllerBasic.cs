using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerBasic : MonoBehaviour {
	public int damage = 10;
	public PlayerData data;
	public Transform playerCam;
	public Transform playerCamPos;
	
	private GameObject[] mainCam;
	private GameObject mainScript;
	private bool resFlag = false;
	private float timer = 0;
	private Transform meshChild;
	private bool locked = false; 
	
	void Awake() {
		mainCam = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		mainScript = GameObject.FindGameObjectWithTag(Tags.gameController);
		
		//meshChild = this.transform.FindChild("Mesh");
		
		if(!networkView.isMine) {
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
			
			if(!Network.isServer)
				TurnOffMainCameras();
		}
		else {
			TurnOffMainCameras(); 
			
			playerCamPos.gameObject.SetActive(true);
			playerCam = GameObject.FindGameObjectWithTag(Tags.playerCamera).transform;
			playerCam.GetComponent<Camera>().enabled = true;
			playerCam.GetComponent<AudioListener>().enabled = true;
			playerCam.GetComponent<ThirdPersonCamera>().enabled = true;
		}
	}
	
	void Start() {
		LockMousePosition();
	}
	
	void TurnOffMainCameras() {
		foreach( GameObject cam in mainCam )
			cam.SetActive(false);
	}
	
	void TurnONMainCameras() {
		foreach( GameObject cam in mainCam )
			cam.SetActive(true);
	}
	
	void Update() {
		if(resFlag)
			Respawn();
		
		if(networkView.isMine)
			InputCheck();
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
	
	private void InputCheck() {
		if (Input.GetKeyDown(KeyCode.Escape))
			CheckCursorLock();
	}
	
	private void CheckCursorLock() {
		if (locked)
			UnlockMousePosition();
		else
			LockMousePosition();
	}
	
	private void LockMousePosition() {
		locked = true;
		Screen.lockCursor = true;
	}
	private void UnlockMousePosition() {
		locked = false;
		Screen.lockCursor = false;
	}
	
	void Hit(object[] package) {
		data.health -= (int)package[0];
		if(data.health <= 0) {
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
			
			List<PlayerData> playerList = mainScript.GetComponent<PlayerList>().playerList;
			int id = playerList.FindIndex(player => player.id == networkView.viewID);
			playerList[id].deaths++;			
			networkView.RPC ("UpdateDeath", RPCMode.Others, networkView.viewID);
			
			id = playerList.FindIndex(player => player.id == (NetworkViewID)package[1]);	
			if(id!= -1) {
				playerList[id].kills++;	
				networkView.RPC ("UpdateKill", RPCMode.Others, (NetworkViewID)package[1]);
			}	
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
			data.isAlive = !data.isAlive;
			collider.enabled = !collider.enabled;
			GetComponent<MovementBasic>().enabled = !GetComponent<MovementBasic>().enabled;
			GetComponent<Shooting>().enabled = !GetComponent<Shooting>().enabled;
			//meshChild.renderer.enabled = !meshChild.renderer.enabled;
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