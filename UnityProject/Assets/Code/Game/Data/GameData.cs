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

public class GameData {
	
	public const int BASE_MAX_HEALTH = 2000;

    public const float CRYSTAL_EATING_TIME = 2f;
	
	public const int LEVEL_MAIN_MENU = 1;
	public const int LEVEL_CRYSTAL_CAVERNS_SERVER = 2;
	public const int LEVEL_CRYSTAL_CAVERNS_CLIENT = 3;
	public const int LEVEL_DEATH_MATCH_CLIENT = 4;
	public const int LEVEL_DEATH_MATCH_SERVER = 5;
	public const int LEVEL_DISCONNECTED = 1;
	
	public static int NUMBER_OF_PLAYERS_A;
	public static int NUMBER_OF_PLAYERS_B;
	public static int NUMBER_OF_CONNECTING_PLAYERS = 0;
	
	public static string SERVER_NAME;
	public static string PLAYER_NAME;
	public static string CURRENT_LEVEL_NAME;
	
	public static bool DRAW_CHAT;
	public static bool DRAW_STATS;
    public static bool DRAW_MINIMAP;
	
	public static Team ACTUAL_CLIENT_TEAM;
}
