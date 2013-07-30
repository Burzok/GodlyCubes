using UnityEngine;
using System.Collections;

public class TowerLifeServer : MonoBehaviour {
	public int maxhealth = 500;
	public int health;
	public bool isAlive = true;
	
	void Awake() {
		health = maxhealth;
	}
	
	void Hit(object[] package) {
		health -= (int)package[0];
		if(health <= 0){
			isAlive = false;
			GetComponent<BoxCollider>().enabled = false;
			transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
			
			//Place for animation
			
			if(GetComponent<TowerReloaderServer>().bullet)
				Network.Destroy(GetComponent<TowerReloaderServer>().bullet.gameObject);
			Network.Destroy(this.gameObject);
		}
	}
}
