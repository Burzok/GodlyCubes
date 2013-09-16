using UnityEngine;

public class PlayerData : MonoBehaviour {
	public NetworkViewID id;
	public string playerName;
	
	public Vector3 color;
	public Team team;
	
	public bool isAlive = true;
	
	public PlayerStats stats;
	public PlayerSkills skills;
	
	public float respawnTime = 3.0f;
	public Vector3 respawnPosition;
	
	public int kills;
	public int deaths;
	public int assist;	
	
	void OnEnable() {
		PlayerManager.singleton.AddPlayer(this);
	}
	
	void OnDisable() {
		PlayerManager.singleton.RemovePlayer(this);
	}
}
