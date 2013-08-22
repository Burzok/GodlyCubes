using UnityEngine;
using System.Collections;

public enum Team {
	TEAM_A,
	TEAM_B,
	TEAM_NEUTRAL
}

public enum Map { 
	CrystalCaverns,
	BurzokGrounds
}

public class GameData {
	
	public const int LEVEL_CRYSTAL_CAVERNS_SERVER = 2;
	public const int LEVEL_CRYSTAL_CAVERNS_CLIENT = 3;
	
	public static int NUMBER_OF_PLAYERS_A;
	public static int NUMBER_OF_PLAYERS_B;
	
	public static string SERVER_NAME;
	public static string PLAYER_NAME;
	
	public static Team ACTUAL_CLIENT_TEAM;
	
}
