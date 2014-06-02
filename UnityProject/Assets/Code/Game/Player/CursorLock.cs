using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorLock : MonoBehaviour {
	private bool locked; 
	
	void Awake() {
		locked = false;	
	}

	private void Start()
	{
		if(networkView.isMine)
		{
			LockMousePosition();
		}
	}
	
	void Update() {
		if(networkView.isMine == true)
			InputCheck();
	}
	
	private void InputCheck() {
		if (Input.GetKeyDown(KeyCode.Escape))
			CheckCursorLock();
	}
	
	private void CheckCursorLock() {
		if (locked == true)
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