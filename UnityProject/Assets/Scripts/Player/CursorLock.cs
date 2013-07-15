using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorLock : MonoBehaviour {
	private bool locked = false; 
	
	void Awake() {
		if(!networkView.isMine) { // usune to jak przerobie instantiate graczy
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
	}
	
	void Start() {
		LockMousePosition();
	}
	
	void Update() {
		if(networkView.isMine)
			InputCheck();
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
}