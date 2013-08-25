using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public Transform playerCamPos;
	
	private Transform playerCam;
	
	void Start() {
		if(networkView.isMine == true) {
			playerCamPos.gameObject.SetActive(true);
			playerCam = GameObject.FindGameObjectWithTag(Tags.playerCamera).transform;
			playerCam.GetComponent<Camera>().enabled = true;
			playerCam.GetComponent<AudioListener>().enabled = true;
			playerCam.GetComponent<ThirdPersonCamera>().enabled = true;
		}
	}
}
