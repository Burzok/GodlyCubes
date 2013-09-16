using UnityEngine;
using System.Collections;

public class SkillControler : MonoBehaviour {

	PlayerSkills skills;
	
	void Awake() {
		skills = GetComponent<PlayerSkills>();
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.LogWarning("key 1");
			string skillName = SkillSelect.singleton.keyBinding[1];
			Debug.LogWarning(skillName);
			if(skills.skill.ContainsKey(skillName))
				skills.skill[skillName].Use();
			else
				Debug.LogError("BRAK SKILLA W DICTIONARY");
		}
	}
}
