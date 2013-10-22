using UnityEngine;
using System.Collections;

public class CrystalColorClient : MonoBehaviour {

    public Material []crystalMaterials;

    private Renderer crystalRenderer;
    private PlayerStats playerStats;
    private PlayerList playerList;

    void Awake() {
        playerStats = gameObject.GetComponent<PlayerStats>();
        playerList = playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
    }

    [RPC]
    void ChangePlayerCrystalColor(int aquiredType, NetworkViewID crystalPlayerID) {
        CrystalType crystal = (CrystalType)aquiredType;
        GameObject player = playerList.FindPlayer(ref crystalPlayerID);
        crystalRenderer = player.transform.Find("Animator").Find("Crystals").GetComponent<Renderer>();

        if(crystal == CrystalType.Strength)
            crystalRenderer.material = crystalMaterials[0];

        if (crystal == CrystalType.Wisdom)
            crystalRenderer.material = crystalMaterials[1];

        if (crystal == CrystalType.Hardness)
            crystalRenderer.material = crystalMaterials[2];          
    }
}
