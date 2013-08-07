using UnityEngine;
using System.Collections;

public class GUIBar : MonoBehaviour {
	public Texture2D texture;
	public Material material;
	public float health = 100;
	
	private PlayerData data;
	
	public float x = 0;
	public float y = 0;
	public float w = 0;
	public float h = 0;
	
	void OnGUI() {
		if(Event.current.type.Equals(EventType.Repaint)) {
			Rect box = new Rect(x, y, w, h);
			Graphics.DrawTexture(box, texture, material);
		}
	}
	
	void Update() {
		float helthy = 1-(health/100);
		if(helthy == 0)
			helthy = 0.1f;
		material.SetFloat("_Cutoff", helthy);
	}
}
