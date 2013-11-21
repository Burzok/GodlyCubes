using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void SkillsToChose();

public class SkillSelect : SingletonComponent<SkillSelect> {
	
	public Dictionary<int, string> keyBinding;
	
	new void Awake() {
        base.Awake();
		keyBinding = new Dictionary<int, string>();
	}
	
	public void AddSkill(string skillName, int keyNumber) {
		Debug.LogWarning("key: " + keyNumber + "name: " + skillName);
		keyBinding.Add(keyNumber, skillName);
	}
	
	public void removeSkill(int keyNumber) {
		Debug.LogWarning("remove key: " + keyNumber);
        try
        {
            keyBinding.Remove(keyNumber);
        }
        catch (ArgumentNullException e)
        {
            Debug.LogWarning("Nie ma elementu");
        }
	}
	
	public void skillsToChose() {
		foreach(KeyValuePair<int,string> skill in keyBinding) {
			switch(skill.Value) {
				case Skills.HEAL:
					PlayerSkills skills = PlayerManager.instance.myPlayer.skills;
				
					skills.SetSkill(skill.Value, Heal.CreateInstance(Skills.HEAL));
					skills.SetSkillData(Skills.HEAL, PlayerManager.instance.myPlayer.stats);
					break;
				default:
					break;
			}
		}
	}
}
