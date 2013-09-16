using UnityEngine;

public delegate void Draw();

public class DrawUI : MonoBehaviour {
	
	public Draw drawUI;
	
	public static DrawUI singleton;
	
	void Awake() {
		drawUI = Dumy;
	}
	
	void OnEnable() {
		if (singleton == null)
			singleton = this;
	}
	
	void OnGUI() {
		drawUI();
	}
	
	private void Dumy()
	{}
}
