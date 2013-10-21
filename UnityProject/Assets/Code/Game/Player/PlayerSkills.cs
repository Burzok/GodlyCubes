using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour {
	public Dictionary<string, ScriptableObject> skill;
	
	void Awake() {
		skill = new Dictionary<string, ScriptableObject>();
	}
	
	public void SetSkill(string skillName, ScriptableObject skill) {
		this.skill.Add(skillName, skill);
	}
	
	public void UseSkill(string skillName) {
		if(skill.ContainsKey(skillName) == true)
		{
			ISkill castSkill = skill[skillName] as ISkill;
			if(castSkill.InUse() == false)
				castSkill.Use();
		}
		else
			Debug.LogError("BRAK SKILLA W DICTIONARY");
	}
	
	public void SetSkillData(string skillName, PlayerStats stats) {
		ISkill castSkill = skill[skillName] as ISkill;
		castSkill.SetData(stats);
	}	
}