using UnityEngine;
using System.Collections;

public class CrystalColorServer : MonoBehaviour
{
    private PlayerStats playerStats;
    private CrystalServer crystalServer;
    private int crystalType;
    private bool colorChanged;
    private int currentValue;

    void Awake() {
        playerStats = gameObject.GetComponent<PlayerStats>();
        colorChanged = false;
        currentValue = 0;
    }

    void Update() {
       crystalType = CheckPrimaryStat();
       if(colorChanged && crystalType != -1) {
           networkView.RPC("ChangePlayerCrystalColor", RPCMode.Others, crystalType, networkView.viewID);
           colorChanged = false;
       }
    }

    int CheckPrimaryStat() {
        if (playerStats.strength > playerStats.wisdom && playerStats.strength > playerStats.hardness &&
            playerStats.strength != currentValue) {
            currentValue = playerStats.strength;
            colorChanged = true;
            return (int)CrystalType.Strength;
        }

        if (playerStats.wisdom > playerStats.strength && playerStats.wisdom > playerStats.hardness &&
            playerStats.wisdom != currentValue) {
            currentValue = playerStats.wisdom;
            colorChanged = true;
            return (int)CrystalType.Wisdom;
        }

        if (playerStats.hardness > playerStats.wisdom && playerStats.hardness > playerStats.strength &&
            playerStats.wisdom != currentValue) {
            currentValue = playerStats.hardness;
            colorChanged = true;
            return (int)CrystalType.Hardness;
        }

        return -1;
    }

    [RPC]
    void ChangePlayerCrystalColor(int aquiredType, NetworkViewID crystalPlayerID) {}
}