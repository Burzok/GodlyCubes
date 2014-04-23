using UnityEngine;
using System.Collections;

public class BaseLifeClient : MonoBehaviour {
	public Team team;
	
	private FinishStateUI finishStateUI;
	
	void Awake() {
		finishStateUI = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FinishStateUI>();
	}
	
	[RPC]
	public void SetStateALostBWin() {
		team = GameData.instance.gameDataAsset.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A) 
			finishStateUI.SetLoseState();
		else if(team == Team.TEAM_B)
            finishStateUI.SetWinState();
	}
	
	[RPC]	
	public void SetStateBLostAWin() {
		team = GameData.instance.gameDataAsset.ACTUAL_CLIENT_TEAM;
		
		if(team == Team.TEAM_A)
            finishStateUI.SetWinState();
		else if(team == Team.TEAM_B)
            finishStateUI.SetLoseState();
	}
}