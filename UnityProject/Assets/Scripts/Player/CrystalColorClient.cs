using UnityEngine;
using System.Collections;

public class CrystalColorClient : MonoBehaviour {

    private Renderer crystalRenderer;
    private PlayerStats playerStats;
    private Material crystalMaterial;

    void Awake() {
        crystalRenderer = transform.Find("Animator").Find("Crystals").GetComponent<Renderer>();
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

	void Update () {
        
	}
}
