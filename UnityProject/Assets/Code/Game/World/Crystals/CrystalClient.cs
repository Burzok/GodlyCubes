using UnityEngine;
using System.Collections;

public class CrystalClient : MonoBehaviour {
	private PlayerList playerList;
	private bool eatingKeyPressed;
    private bool eatingInProgress;
    private bool requestSent;
	
	private CrystalType type;
	private int increaseStatValue;
    private Collider collidingPlayer;
	
	private GameObject increasingPlayer;
	private PlayerStats increasingPlayerStats;
    private Rotator myPlayerRotator;
    private Animator animator;
    private CrystalBarUI crystalBarUI;
	
	void Awake() {
		playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
        crystalBarUI = GameObject.Find("Camera_GUI").GetComponent<CrystalBarUI>();
        requestSent = false;
        eatingInProgress = false;
	}
	
	void FixedUpdate() {
		if(Input.GetKeyDown(KeyCode.E))
			eatingKeyPressed = true;
		else
			eatingKeyPressed = false;

        if (eatingInProgress) {
            Input.ResetInputAxes();
            myPlayerRotator.enabled = false;
        }
           
	}
	
	void OnTriggerStay(Collider player) {
		if(player.networkView.isMine) {
            if (eatingKeyPressed && !requestSent) {
                collidingPlayer = player;

                if(animator == null)
                    animator = collidingPlayer.transform.FindChild("Animator").GetComponent<Animator>();

                animator.SetBool("Eat", true);
                networkView.RPC("RequestCrystalEating", RPCMode.Server, player.networkView.viewID);               
                requestSent = true;
            }
		}
	}

    [RPC]
    void ReplyCrystalEating() {
        if(myPlayerRotator == null)
            myPlayerRotator = PlayerManager.instance.myPlayer.GetComponent<Rotator>();
        eatingInProgress = true;
        crystalBarUI.eatingInProgress = true;
    }

    [RPC]
    void StopCrystalEating() {
        eatingInProgress = false;
        crystalBarUI.eatingInProgress = false;
        myPlayerRotator.enabled = true;
        animator.SetBool("Eat", false);
        networkView.RPC("RequestStatIncrease", RPCMode.Server, collidingPlayer.networkView.viewID);
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
    void DestroyCrystal() {
        Destroy(this.gameObject);
    }
	
	[RPC]
	void RequestStatIncrease(NetworkViewID increasingPlayerID) 
	{}

    [RPC]
    void RequestCrystalEating(NetworkViewID increasingPlayerID) 
	{}
}
