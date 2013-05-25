using UnityEngine;
using System.Collections;

public class BaseLife : MonoBehaviour
{
public int maxhealth = 2000;
	
	public int health;
	
	void Awake() {
		health = maxhealth;
	}
	
	void Hit(object[] package) {
		health -= (int)package[0];
		if(health <= 0){
			Network.Destroy(this.gameObject);
		}
	}
}

