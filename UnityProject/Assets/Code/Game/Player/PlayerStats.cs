using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public float currentHealth = 100;
	public float maxHealth = 100;
	
	public int currentEnergy = 100;
	public int maxEnergy = 100 ;
	
	public int currentUltiPower = 10;
	public int maxUltiPower = 10;
	
	public int strength = 10;
	public int wisdom = 10;
	
	public int hardness = 0;
	public int absorption = 0;
	
	public int penetration = 0;
	public int focus = 0;

	public GameObject enemyProgressBarPrefab;
	private GameObject cam;

	private UIProgressBar progressBar;
	private UIProgressBar enemyProgressBar;

	void Start() {
		currentHealth = maxHealth;
		currentEnergy = maxEnergy;
		currentUltiPower = maxUltiPower;

		cam = GameObject.Find("Camera");

		if(!Network.isServer && networkView.isMine)
			progressBar = GameObject.Find("Foreground").GetComponent<UIProgressBar>();
		else if(!Network.isServer) {
			GameObject enemyBarInstance = Instantiate(enemyProgressBarPrefab) as GameObject;
			enemyBarInstance.transform.parent = cam.transform;;
			enemyBarInstance.GetComponent<EnemyHealthBarPositionUI>().enemy = gameObject;

			enemyProgressBar = enemyBarInstance.GetComponentInChildren<UIProgressBar>();
		}
	}

	void Update() {
		if(!Network.isServer && networkView.isMine)
			progressBar.value = currentHealth/maxHealth;
		else if(!Network.isServer) {
			enemyProgressBar.value = currentHealth/maxHealth;
		}
	}
}
