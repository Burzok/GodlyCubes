using UnityEngine;

public delegate void Draw();

public class DrawUI : SingletonComponent<DrawUI> {
	
	public Draw drawUI;
	
	void OnGUI() {
        if (drawUI != null)
		    drawUI();
	}
}
