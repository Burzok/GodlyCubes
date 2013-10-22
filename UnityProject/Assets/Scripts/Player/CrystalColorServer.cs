using UnityEngine;
using System.Collections;

public class CrystalColorServer : MonoBehaviour
{
    private PlayerStats playerStats;
    private CrystalServer crystalServer;
    private CrystalType crystalType;
    private bool colorChanged;
    private int currentValue;

    void Awake() {
        playerStats = gameObject.GetComponent<PlayerStats>();
        colorChanged = false;
        currentValue = 0;
    }

    void Update() {
       crystalType = CheckPrimaryStat();
       if(colorChanged) {
           networkView.RPC("ChangePlayerCrystalColor", RPCMode.Others, (int)crystalType, networkView.viewID);
           colorChanged = false;
       }
    }

    CrystalType CheckPrimaryStat() {
        if (playerStats.strength > playerStats.wisdom && playerStats.strength > playerStats.hardness &&
            playerStats.strength != currentValue) {
            currentValue = playerStats.strength;
            colorChanged = true;
            return CrystalType.Strength;
        }

        if (playerStats.wisdom > playerStats.strength && playerStats.wisdom > playerStats.hardness &&
            playerStats.wisdom != currentValue) {
            currentValue = playerStats.wisdom;
            colorChanged = true;
            return CrystalType.Wisdom;
        }

        if (playerStats.hardness > playerStats.wisdom && playerStats.hardness > playerStats.strength &&
            playerStats.wisdom != currentValue) {
            currentValue = playerStats.hardness;
            colorChanged = true;
            return CrystalType.Hardness;
        }

        return 0;
    }

    [RPC]
    void ChangePlayerCrystalColor(int aquiredType, NetworkViewID crystalPlayerID) {}
}