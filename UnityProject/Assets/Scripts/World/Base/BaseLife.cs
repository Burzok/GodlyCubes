using UnityEngine;
using System.Collections;

public class BaseLife : MonoBehaviour
{
	public int maxhealth = 2000;
	public Team playerTeam;
	public MenuGUI menuGUI;
	
	public int health;
	
	void Awake() {
		menuGUI = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<MenuGUI>();
		health = maxhealth;
	}
	
	void Hit(object[] package) {
		health -= (int)package[0];
		if(health <= 0) {
			if(name=="BaseA") 
				networkView.RPC("setStateA", RPCMode.Others);
			else if(name=="BaseB") 
				networkView.RPC("setStateB", RPCMode.Others);
			
			Network.Destroy(this.gameObject);
		}
	}
	
	[RPC]	
	public void setStateA() {
		playerTeam = GameData.ACTUAL_CLIENT_TEAM;
		
		if(playerTeam == Team.TEAM_A) 
			menuGUI.SetLoseState();
		else if(playerTeam == Team.TEAM_B) 
			menuGUI.SetWinState();
	}
	
	[RPC]	
	public void setStateB() {
		playerTeam = GameData.ACTUAL_CLIENT_TEAM;
		
		if(playerTeam == Team.TEAM_A) 	
			menuGUI.SetWinState();
		else if(playerTeam == Team.TEAM_B) 
			menuGUI.SetLoseState();
	}
}