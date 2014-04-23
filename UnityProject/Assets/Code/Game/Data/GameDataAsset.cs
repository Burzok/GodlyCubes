using UnityEngine;
using System.Collections;

public enum Team {
	TEAM_A,
	TEAM_B,
	TEAM_NEUTRAL
}

public enum Map { 
	CRYSTAL_CAVERNS,
	DEATH_MATCH,
	BurzokGrounds
}

public class GameDataAsset : ScriptableObject {
	
	public int BASE_MAX_HEALTH = 2000;
	public int OBSTACLE_MAX_HEALTH = 1000;

    public float CRYSTAL_EATING_TIME = 2f;
	
	public int LEVEL_MAIN_MENU = 1;
	public int LEVEL_CRYSTAL_CAVERNS_SERVER = 2;
	public int LEVEL_CRYSTAL_CAVERNS_CLIENT = 3;
	public int LEVEL_DEATH_MATCH_CLIENT = 4;
	public int LEVEL_DEATH_MATCH_SERVER = 5;
	public int LEVEL_DISCONNECTED = 1;
	
	public int NUMBER_OF_PLAYERS_A;
	public int NUMBER_OF_PLAYERS_B;
	public int NUMBER_OF_CONNECTING_PLAYERS = 0;
	
	public string SERVER_NAME;
	public string PLAYER_NAME;
	public string CURRENT_LEVEL_NAME;
	
	public bool DRAW_CHAT;
	public bool DRAW_STATS;
	public bool DRAW_MINIMAP;
	
	public Team ACTUAL_CLIENT_TEAM;

	#if UNITY_EDITOR
		[UnityEditor.MenuItem("Assets/Create/GameData")]
		public static void CreateAsset ()
		{
			ScriptableObjectUtility.CreateAsset<GameDataAsset> ();
		}
	#endif
}
