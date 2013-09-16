﻿using UnityEngine;
using System.Collections.Generic;

public delegate void SkillsToChose();

public class SkillSelect : MonoBehaviour {
	
	public static SkillSelect singleton;
	public Dictionary<int, string> keyBinding;
	
	void Awake() {
		keyBinding = new Dictionary<int, string>();	
	}
	
	void OnEnable() {
		if (singleton == null)
			singleton = this;
	}

	public void AddSkill(string skillName, int keyNumber) {
		Debug.LogWarning("key: " + keyNumber + "name: " + skillName);
		keyBinding.Add(keyNumber, skillName);
	}
	
	public void removeSkill(int keyNumber) {
		Debug.LogWarning("remove key: " + keyNumber);
		keyBinding.Remove(keyNumber);
	}
	
	public void skillsToChose() {
		foreach(KeyValuePair<int,string> skill in keyBinding) {
			switch(skill.Value) {
				case Skills.HEAL:
					Debug.LogWarning("allockate heal");
					PlayerManager.singleton.myPlayer.skills.skill.Add(
							skill.Value, new Heal(PlayerManager.singleton.myPlayer.stats));
					break;
				default:
					break;
			}
		}
	}
}
