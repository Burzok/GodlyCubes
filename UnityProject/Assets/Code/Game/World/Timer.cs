using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float timer;
	private bool play;
	
	void Awake() {
		timer = 0;
		play = false;
	}
	
	void Update() {
		if (play == true)
			timer += Time.deltaTime;
	}
	
	public void StartTimer() {
		play = true;
		timer = 0;
	}
	
	public void Reset() {
		play = true;
	}
	
	public void Stop() {
		play = false;
	}
	
	public bool CheckTime(ref float time) {
		if (timer >= time)
			return true;
		else 
			return false;
	}
}
