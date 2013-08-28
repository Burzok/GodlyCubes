using UnityEngine;
using System.Collections;

public class BaseLifeClient : MonoBehaviour {
	public Team team;
	
	private MenuUI menuGUI;
	
	void Awake() {
		menuGUI = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<MenuUI>();
	}
	
	[RPC]
	public void SetStateALostBWin() {
		team = GameData.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A) 
			menuGUI.SetLoseState();
		else if(team == Team.TEAM_B) 
			menuGUI.SetWinState();
	}
	
	[RPC]	
	public void SetStateBLostAWin() {
		team = GameData.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A) 	
			menuGUI.SetWinState();
		else if(team == Team.TEAM_B) 
			menuGUI.SetLoseState();
	}
}