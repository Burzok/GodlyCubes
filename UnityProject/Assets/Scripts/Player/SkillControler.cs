using UnityEngine;
using System.Collections;

public class SkillControler : MonoBehaviour {

	PlayerSkills skills;
	
	void Awake() {
		skills = GetComponent<PlayerSkills>();
	}
	
	void Update() {
		 if(Input.GetKeyDown(KeyCode.Alpha1))
			skills.skill1.Use();
	}
}
