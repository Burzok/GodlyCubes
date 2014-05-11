using UnityEngine;
using System.Collections;

public class EnemyHealthBarPositionUI : MonoBehaviour {
	
	public GameObject enemy;
	private GameObject myPlayer;
	private Camera playerCamera;
	private Camera guiCamera;
	
	private Vector3 position;
	private float distance;
	private float size;
	
	void Start() {
		playerCamera = NGUITools.FindCameraForLayer(enemy.layer);
		guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		transform.localScale = Vector3.one;
	}
	
	void LateUpdate() {
		if (PlayerManager.instance.myPlayer) {
			UpdateBarPosition();
			UpdateBarSize();
		}
	}
	
	void UpdateBarPosition() {
		position = playerCamera.WorldToViewportPoint(enemy.transform.position);
		position = guiCamera.ViewportToWorldPoint(position);
		
		position.z = 0f;
		position.y += 0.1f;
		transform.position = position;
		if(enemy.transform.FindChild("Animator/Player").renderer.isVisible == false) {
			gameObject.GetComponent<UISprite>().enabled = false;
			gameObject.transform.GetChild (0).GetComponent<UISprite>().enabled = false;	
		}
		else {
			gameObject.GetComponent<UISprite>().enabled = true;
			gameObject.transform.GetChild (0).GetComponent<UISprite>().enabled = true;
		}
	}
	
	void UpdateBarSize() {
		distance = Vector3.Distance(enemy.transform.position, PlayerManager.instance.myPlayer.transform.position);
		size = 1/Mathf.Log10(distance);

		if(size >= 1)
			size = 1;

		transform.localScale = new Vector3(size, size, 1f);	
	}
}
