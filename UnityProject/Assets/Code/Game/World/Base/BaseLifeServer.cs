using UnityEngine;
using System.Collections;

public class BaseLifeServer : MonoBehaviour {
	public Team baseTeam;
	
	public int currentHealth;
	
	void Awake() {
		currentHealth = GameData.BASE_MAX_HEALTH;
	}
	
	void Start() {
		baseTeam = GetComponent<BaseTeamChoserServer>().teamSelect;	
	}
	
	void Hit(object[] package) {
		currentHealth -= (int)package[0];
		
		if(currentHealth <= 0) {
			if(baseTeam == Team.TEAM_A) 
				networkView.RPC("SetStateALostBWin", RPCMode.Others);
			else if(baseTeam == Team.TEAM_B)
				networkView.RPC("SetStateBLostAWin", RPCMode.Others);
			
			Network.Destroy(this.gameObject);
		}
	}
	
	[RPC]
	public void SetStateALostBWin() 
	{}
	
	[RPC]
	public void SetStateBLostAWin() 
	{}
}