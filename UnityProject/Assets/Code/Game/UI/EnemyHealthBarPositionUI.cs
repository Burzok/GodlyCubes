using UnityEngine;
using System.Collections;

public class EnemyHealthBarPositionUI : MonoBehaviour {

	public GameObject enemy;
	private Camera playerCamera;
	private Camera guiCamera;

	private Vector3 position;

	void Start() {
		playerCamera = NGUITools.FindCameraForLayer(enemy.layer);
		guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		transform.localScale = Vector3.one;
	}

	void LateUpdate() {
		position = playerCamera.WorldToViewportPoint(enemy.transform.position);
		position = guiCamera.ViewportToWorldPoint(position);

		position.z = 0f;
		position.y+= 0.2f;
		transform.position = position;
		if(enemy.transform.FindChild("Animator/Player").renderer.isVisible == false) {
			Debug.LogWarning(gameObject.name);
			Debug.LogWarning(gameObject.transform.GetChild (0).name);
			gameObject.GetComponent<UISprite>().enabled = false;
			gameObject.transform.GetChild (0).GetComponent<UISprite>().enabled = false;	
		}
		else {
			gameObject.GetComponent<UISprite>().enabled = true;
			gameObject.transform.GetChild (0).GetComponent<UISprite>().enabled = true;
		}
	}
}
