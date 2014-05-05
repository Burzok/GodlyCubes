using UnityEngine;
using System.Collections;

public class HealthBarUI : MonoBehaviour {
	PlayerManager playerManager;
	UIProgressBar progressBar;
	float barHeight;

	void Awake() {
		playerManager = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerManager>();
		progressBar =  GetComponent<UIProgressBar>();
		barHeight = Screen.height - 20;		
		transform.position.Set (transform.position.x, barHeight, transform.position.z);
	}
	void Update () {
		if(playerManager.myPlayer != null) {
			if(playerManager.myPlayer.isAlive == false)
				progressBar.alpha = 0f;
			else
				progressBar.alpha = 0.65f;	
		}
	}
}
