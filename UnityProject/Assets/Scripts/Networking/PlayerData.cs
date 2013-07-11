using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	public NetworkViewID id;
	
	public Transform localTransform;
	public string playerName;
	
	public Vector3 color;
	public Team team;
	
	public bool isAlive = true;
	
	public int health = 100;
	public int maxHealth = 100;
	
	public float respawnTime = 3.0f;
	public Vector3 respawnPosition;
	
	public int kills;
	public int deaths;
	public int assist;	
}
