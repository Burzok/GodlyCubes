using UnityEngine;
using System.Collections;

public enum Team {
	TEAM_A,
	TEAM_B,
	TEAM_NEUTRAL
}

public enum Map { 
	CRYSTAL_CAVERNS,
	BurzokGrounds
}

public class GameData {
	
	public static bool DRAW_CHAT;
	public static bool DRAW_STATS;
	
	public const int LEVEL_MAIN_MENU = 1;
	public const int LEVEL_CRYSTAL_CAVERNS_SERVER = 2;
	public const int LEVEL_CRYSTAL_CAVERNS_CLIENT = 3;
	public const int LEVEL_DISCONNECTED = 1;
	
	public static int NUMBER_OF_PLAYERS_A;
	public static int NUMBER_OF_PLAYERS_B;
	public static int NUMBER_OF_CONNECTING_PLAYERS = 0;
	
	public static string SERVER_NAME;
	public static string PLAYER_NAME;
	public static string CURRENT_LEVEL_NAME;
	
	public static Team ACTUAL_CLIENT_TEAM;
	
	
	
	
}
