using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public float speed = 0.1f;
	
	// Use this for initialization
	void Start () {
		networkView.observed = this;
	}
	
	// Update is called once per frame
	void Update () {
		if( networkView.isMine){
			if( Input.GetKey(KeyCode.D) ){
				transform.position += new Vector3 (1f, 0, 0f) * Time.fixedDeltaTime * speed;
			}
			
			if( Input.GetKey(KeyCode.A) ){
				transform.position -= new Vector3 (1f, 0, 0f) * Time.fixedDeltaTime * speed;
			}
			
			if( Input.GetKey(KeyCode.W) ){
				transform.position += new Vector3 (0f, 0, 1f) * Time.fixedDeltaTime * speed;
			}
			
			if( Input.GetKey(KeyCode.S) ){
				transform.position -= new Vector3 (0f, 0, 1f) * Time.fixedDeltaTime * speed;
			}
		}
	}
}
