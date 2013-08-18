using UnityEngine;
using System.Collections;

public class CrystalServer : MonoBehaviour {
	public CrystalType type;	
	public int minimumValueRange = 10;
	public int maximumValueRange = 20;
	public CrystalSpawnerServer crystalSpawnerServer;
	private int increaseStatValue;	
	
	private PlayerList playerList;
	
	private GameObject increasingPlayer;
	private PlayerStats increasingPlayerStats;
	
	void Awake() {		
		playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
		increaseStatValue = Random.Range(minimumValueRange, maximumValueRange);
		
		if(minimumValueRange > maximumValueRange) {
			int tempValueRange = maximumValueRange;
			maximumValueRange = minimumValueRange;
			minimumValueRange = tempValueRange;
		}
	}
	
	[RPC]
	void RequestStatIncrease(NetworkViewID increasingPlayerID) {
		increasingPlayer = playerList.FindPlayer(ref increasingPlayerID);
		increasingPlayerStats = increasingPlayer.GetComponent<PlayerData>().stats;
		
		ApplyStatAccordingToType();
 		networkView.RPC("ReplyStatIncrease", RPCMode.Others, increasingPlayerID, (int)type, increaseStatValue);
		
		Network.Destroy(this.gameObject);
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
	void ReplyStatIncrease(NetworkViewID increasingPlayerID, int aquiredType, int aquiredIncreaseStatValue)
	{}
}
