using UnityEngine;
using System.Collections;

public class CrystalColorClient : MonoBehaviour {

    public Material []crystalMaterials;

    private Renderer crystalRenderer;
    private PlayerList playerList;

    void Awake() {
        playerList = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerList>();
    }

    [RPC]
    void ChangePlayerCrystalColor(int aquiredType, NetworkViewID crystalPlayerID) {
        CrystalType crystal = (CrystalType)aquiredType;
        GameObject player = playerList.FindPlayer(ref crystalPlayerID);
        crystalRenderer = player.transform.Find("Animator").Find("Crystals").GetComponent<Renderer>();

        if (crystal == CrystalType.Strength) {
            crystalRenderer.material = crystalMaterials[0];

            gameObject.GetComponent<PlayerData>().color.x = crystalMaterials[0].color.r;
            gameObject.GetComponent<PlayerData>().color.y = crystalMaterials[0].color.g;
            gameObject.GetComponent<PlayerData>().color.z = crystalMaterials[0].color.b;
        }

        if (crystal == CrystalType.Wisdom) {
            crystalRenderer.material = crystalMaterials[1];

            gameObject.GetComponent<PlayerData>().color.x = crystalMaterials[1].color.r;
            gameObject.GetComponent<PlayerData>().color.y = crystalMaterials[1].color.g;
            gameObject.GetComponent<PlayerData>().color.z = crystalMaterials[1].color.b;
        }

        if (crystal == CrystalType.Hardness) {
            crystalRenderer.material = crystalMaterials[2];

            gameObject.GetComponent<PlayerData>().color.x = crystalMaterials[2].color.r;
            gameObject.GetComponent<PlayerData>().color.y = crystalMaterials[2].color.g;
            gameObject.GetComponent<PlayerData>().color.z = crystalMaterials[2].color.b;
        }
    }
}
