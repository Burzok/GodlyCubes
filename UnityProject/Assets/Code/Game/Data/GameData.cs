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

public class GameData : SingletonComponent<GameData> {

	public static int BASE_MAX_HEALTH = 2000;
	public static int OBSTACLE_MAX_HEALTH = 1000;
	
	public static float CRYSTAL_EATING_TIME = 2f;
	
	public static int LEVEL_MAIN_MENU = 1;
	public static int LEVEL_CRYSTAL_CAVERNS_SERVER = 2;
	public static int LEVEL_CRYSTAL_CAVERNS_CLIENT = 3;
	public static int LEVEL_DEATH_MATCH_CLIENT = 4;
	public static int LEVEL_DEATH_MATCH_SERVER = 5;
	public static int LEVEL_DISCONNECTED = 1;
	
	public static int NUMBER_OF_CONNECTING_PLAYERS = 0;
	
	public static string SERVER_NAME = "Server";
	public static string PLAYER_NAME = "Player";
	public static string CURRENT_LEVEL_NAME ="Deathmatch";
	
	public static Team ACTUAL_CLIENT_TEAM;
	public static int NUMBER_OF_PLAYERS_A = 0;
	public static int NUMBER_OF_PLAYERS_B = 0;
}
