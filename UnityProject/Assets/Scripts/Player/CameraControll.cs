using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour {
	public Transform playerCamPos;
	
	private Transform playerCam;
	private GameObject[] mainCameras;
	
	void Awake() {
		mainCameras = GameObject.FindGameObjectsWithTag(Tags.mainCamera);
		
		if(!networkView.isMine) {
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
	
	void TurnOffMainCameras() {
		foreach( GameObject cam in mainCameras )
			cam.SetActive(false);
	}
	
	void TurnONMainCameras() {
		foreach( GameObject cam in mainCameras )
			cam.SetActive(true);
	}
}
