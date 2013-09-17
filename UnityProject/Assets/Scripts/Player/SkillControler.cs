using UnityEngine;
using System.Collections;

public class SkillControler : MonoBehaviour {
	
	public static SkillControler singleton;
	private PlayerSkills skills;
	public bool skillInUse;
	
	void OnEnable() {
		if (singleton == null)
			singleton = this;
	}
	
	void Awake() {
		skills = GetComponent<PlayerSkills>();
		skillInUse = false;
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(skillInUse == false)
			{
				skillInUse = true;
				string skillName = SkillSelect.singleton.keyBinding[1];
				skills.UseSkill(skillName);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(skillInUse == false)
			{
				skillInUse = true;
				string skillName = SkillSelect.singleton.keyBinding[2];
				skills.UseSkill(skillName);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			if(skillInUse == false)
			{
				skillInUse = true;
				string skillName = SkillSelect.singleton.keyBinding[3];
				skills.UseSkill(skillName);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			if(skillInUse == false)
			{
				skillInUse = true;
				string skillName = SkillSelect.singleton.keyBinding[4];
				skills.UseSkill(skillName);
			}
		}
	}
}
