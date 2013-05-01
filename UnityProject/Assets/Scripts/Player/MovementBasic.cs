using UnityEngine;
using System.Collections;

public class MovementBasic : MonoBehaviour {
	public float forwardSpeed = 20f; 
	
	private float horizontalInput;
	private float verticalInput;
	
	void FixedUpdate() {
		if(networkView.isMine) {
			horizontalInput = Input.GetAxis("Horizontal");
			verticalInput = Input.GetAxis("Vertical");
			
			if ( verticalInput != 0 ) {
				rigidbody.AddForce( transform.forward * Time.deltaTime * forwardSpeed * verticalInput, 
									ForceMode.VelocityChange);
			}
			
			if ( horizontalInput != 0 ) {
				rigidbody.AddForce( transform.right * Time.deltaTime * forwardSpeed * horizontalInput, 
									ForceMode.VelocityChange);
			}
		}
		
	}
}
