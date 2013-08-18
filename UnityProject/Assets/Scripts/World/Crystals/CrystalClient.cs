using UnityEngine;
using System.Collections;

public class CrystalClient : MonoBehaviour {
	private PlayerList playerList;
	private bool eatingKeyPressed;
	
	private CrystalType type;
	private int increaseStatValue;
	
	private GameObject increasingPlayer;
	private PlayerStats increasingPlayerStats;
	
	void Awake() {
		playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
	}
	
	void FixedUpdate() {
		if(Input.GetKeyDown(KeyCode.E))
			eatingKeyPressed = true;
		else
			eatingKeyPressed = false;
	}
	
	void OnTriggerStay(Collider player) {
		if(player.networkView.isMine) {
			if(eatingKeyPressed)
				networkView.RPC("RequestStatIncrease", RPCMode.Server, player.networkView.viewID);
		}
	}	
	
	[RPC]
	void ReplyStatIncrease(NetworkViewID increasingPlayerID, int aquiredType, int aquiredIncreaseStatValue) {
		type = (CrystalType)aquiredType;
		increaseStatValue = aquiredIncreaseStatValue;
		increasingPlayer = playerList.FindPlayer(ref increasingPlayerID);
		increasingPlayerStats = increasingPlayer.GetComponent<PlayerData>().stats;
		
		ApplyStatAccordingToType();
	}
	
	void ApplyStatAccordingToType() {
		if(type == CrystalType.Health) 
			increasingPlayerStats.maxHealth += increaseStatValue;
		
		if(type == CrystalType.Energy) 
			increasingPlayerStats.maxEnergy += increaseStatValue;
		
		if(type == CrystalType.UltiPower) 
			increasingPlayerStats.maxUltiPower += increaseStatValue;
		
		if(type == CrystalType.Strength) 
			increasingPlayerStats.strength += increaseStatValue;
		
		if(type == CrystalType.Wisdom) 
			increasingPlayerStats.wisdom += increaseStatValue;
		
		if(type == CrystalType.Hardness) 
			increasingPlayerStats.hardness += increaseStatValue;
		
		if(type == CrystalType.Absorbtion) 
			increasingPlayerStats.absorption += increaseStatValue;
		
		if(type == CrystalType.Penetration) 
			increasingPlayerStats.penetration += increaseStatValue;
		
		if(type == CrystalType.Focus) 
			increasingPlayerStats.focus += increaseStatValue;
	}
	
	[RPC]
	void RequestStatIncrease(NetworkViewID increasingPlayerID) 
	{}
}
