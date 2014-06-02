using UnityEngine;
using System.Collections;

public class CrystalBarUI : MonoBehaviour {
	public Texture2D texture;
	public Material material;  
    public bool eatingInProgress = false;

	private float eatingTime;
    private float timer = 0.01f;
    private CrystalClient crystalClient;
    private bool startIncrementing;
	
	private float x = Screen.height / 3;
    private float y = Screen.width / 2 + 50;
    private float w = Screen.height / 2;
    private float h = 20;

	void Start() {
		eatingTime = GameData.CRYSTAL_EATING_TIME;
	}
	
	void OnGUI() {
        if (Event.current.type.Equals(EventType.Repaint)) {
            if (eatingInProgress && startIncrementing) {
                Rect box = new Rect(x, y, w, h);
                Graphics.DrawTexture(box, texture, material);
            }
        }
	}
	
	void FixedUpdate() {
        if (eatingInProgress) {
            IncrementTimer();
            float timeLeft = (eatingTime - timer) / 2;
            if (timeLeft == 0)
            {
                timeLeft = 0.1f;
                eatingInProgress = false;
            }
            material.SetFloat("_Cutoff", timeLeft);
        }
        else {
            startIncrementing = false;
            timer = 0.01f;
        }
	}

    private void IncrementTimer() {
        startIncrementing = true;
        timer += Time.deltaTime;
    }
}
