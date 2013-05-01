using UnityEngine;
using System.Collections;

public class PlayerGameData : MonoBehaviour {
	public int health = 100;
	public int maxHealth = 100;
	
	public float respawnTime = 3.0f;
	public Vector3 respawnPosition;
	
	public string name;
	public Vector3 color;
	
	public int kills;
	public int deaths;
	public int assist;
}
