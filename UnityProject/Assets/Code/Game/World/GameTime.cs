using UnityEngine;

public class GameTime {
	
	public static bool isPaused;
	
	void Awake() {
		isPaused = false;
	}
	
	public static void PauseGame() {
		isPaused = true;
		Time.timeScale = 0f;
	}
	
	public static void UnPauseGame() {
		isPaused = false;
		Time.timeScale = 1f;
	}
	
	public static void GameSpeed(float scaleValue) {
		if(scaleValue == 0f)
			isPaused = true;
		
		Time.timeScale *= scaleValue;
	}
}
