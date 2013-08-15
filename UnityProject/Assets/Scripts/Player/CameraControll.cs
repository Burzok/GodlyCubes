using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour {
	public Transform playerCamPos;
	
	private Transform playerCam;
	
	void Awake() {
		if(networkView.isMine) {
			playerCamPos.gameObject.SetActive(true);
			playerCam = GameObject.FindGameObjectWithTag(Tags.playerCamera).transform;
			playerCam.GetComponent<Camera>().enabled = true;
			playerCam.GetComponent<AudioListener>().enabled = true;
			playerCam.GetComponent<ThirdPersonCamera>().enabled = true;
		}
	}
}
