using UnityEngine;
using System.Collections;

public class CrystalServer : MonoBehaviour {
	public CrystalType type;	
	public int minimumValueRange = 10;
	public int maximumValueRange = 20;
	public float eatingTime;
	public CrystalSpawnerServer crystalSpawnerServer;
	
    private int increaseStatValue;
    private NetworkViewID eatingPlayerID;
	
	private PlayerList playerList;
	
	private GameObject increasingPlayer;
	private PlayerStats increasingPlayerStats;
    private bool eatingInProgress;
    private float timer;

	void Awake() {
		eatingTime = GameData.CRYSTAL_EATING_TIME;
		playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
		increaseStatValue = Random.Range(minimumValueRange, maximumValueRange);
		
		if(minimumValueRange > maximumValueRange) {
			int tempValueRange = maximumValueRange;
			maximumValueRange = minimumValueRange;
			minimumValueRange = tempValueRange;
		}
        timer = 0;
        eatingInProgress = false;
	}

    void FixedUpdate() {
        if(eatingInProgress) {
            IncrementTimer();
            if (timer >= eatingTime) {
                networkView.RPC("StopCrystalEating", eatingPlayerID.owner);
                eatingInProgress = false;
                timer = 0;
            }
        }
    }

    private void IncrementTimer() {
            timer += Time.deltaTime;
    }

    [RPC]
    void RequestCrystalEating(NetworkViewID aquiredEatingPlayerID) {
        networkView.RPC("ReplyCrystalEating", aquiredEatingPlayerID.owner);
        eatingPlayerID = aquiredEatingPlayerID;
        eatingInProgress = true;
    }

	[RPC]
	void RequestStatIncrease(NetworkViewID increasingPlayerID) {
		increasingPlayer = playerList.FindPlayer(ref increasingPlayerID);
		increasingPlayerStats = increasingPlayer.GetComponent<PlayerData>().stats;
		
		ApplyStatAccordingToType();
 		networkView.RPC("ReplyStatIncrease", RPCMode.Others, increasingPlayerID, (int)type, increaseStatValue);

        networkView.RPC("DestroyCrystal", RPCMode.Others);
        Destroy(this.gameObject);
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
    void DestroyCrystal()  {
        Destroy(this.gameObject);
    }
	
	[RPC]
	void ReplyStatIncrease(NetworkViewID increasingPlayerID, int aquiredType, int aquiredIncreaseStatValue)	{}

    [RPC]
    void ReplyCrystalEating() {}

    [RPC]
    void StopCrystalEating() {}
}
