using UnityEngine;
using System.Collections;

//Version 1.2

public class ScreenDrops : MonoBehaviour {
	
	public Camera cam;
	
	public void Start() {

		if (cam==null) {
			cam = Camera.main;
		}
		
		if (cam==null) {
			this.enabled = false;	
		} else {
			if (cam.nearClipPlane>=1f) {
				Debug.LogWarning("ScreenDrops::Start() Camera nearClipPlane should be lower than 1.0");
			}
		}
		
		transform.localPosition = Vector3.forward;
		transform.localEulerAngles = Vector3.zero;
		
		
	}
	
    public void Update() {
		if (cam.pixelHeight>=cam.pixelWidth) {
			transform.localScale = 2*Mathf.Tan(cam.fieldOfView/2*Mathf.Deg2Rad) *Vector3.one;
		} else {
			transform.localScale = 2*Mathf.Tan(cam.fieldOfView/2*Mathf.Deg2Rad)*cam.aspect *Vector3.one;
		}
	}
}
