using UnityEngine;
using System.Collections;

public class BaseLifeClient : MonoBehaviour {
	public Team team;
	
	private EndUI endUI;
	
	void Awake() {
		endUI = GameObject.Find("UI Root").GetComponent<EndUI>();
	}
	
	[RPC]
	public void SetStateALostBWin() {
		team = GameData.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A) 
			endUI.LoseState();
		else if(team == Team.TEAM_B)
			endUI.WonState();
	}
	
	[RPC]	
	public void SetStateBLostAWin() {
		team = GameData.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A)
			endUI.WonState();
		else if(team == Team.TEAM_B)
			endUI.LoseState();
	}
}