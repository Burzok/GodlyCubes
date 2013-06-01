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
		if(health <= 0){
			if(name=="BaseA") {
				networkView.RPC("setStateA", RPCMode.All);
			}
			else if(name=="BaseB") {
				networkView.RPC("setStateB", RPCMode.All);
			}
			Network.Destroy(this.gameObject);
		}
	}
	
	[RPC]	
	public void setStateA() {
		playerTeam = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Networking>().actualTeam;
		if(playerTeam == Team.TeamA) {
			menuGUI.SetLoseState();
		}
		else if(playerTeam == Team.TeamB) {
			menuGUI.SetWinState();
		}
	}
	
	[RPC]	
	public void setStateB() {
		playerTeam = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Networking>().actualTeam;
		if(playerTeam == Team.TeamA) {			
			menuGUI.SetWinState();
		}
		else if(playerTeam == Team.TeamB) {
			menuGUI.SetLoseState();
		}
	}
}