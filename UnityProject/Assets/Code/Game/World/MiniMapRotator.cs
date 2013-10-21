using UnityEngine;
using System.Collections;

public class MiniMapRotator : MonoBehaviour {
	
	private GameObject rotatePoint;
	
	void Awake() {
		rotatePoint = GameObject.Find("RotatePoint") ;
	}

	void Update() {
		 transform.RotateAround(rotatePoint.transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
