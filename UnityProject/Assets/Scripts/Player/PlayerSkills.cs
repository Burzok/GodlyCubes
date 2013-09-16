using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour {
	public Dictionary<string, ISkill> skill;
	
	void Awake() {
		skill = new Dictionary<string, ISkill>();
	}
	
	public void SetSkill() {
		
	}
}