using UnityEngine;
using System.Collections;

public delegate void SkillDelegate();

public class SkillSelectUI : MonoBehaviour {
	DrawUI drawUI;
	public SkillDelegate skillDelegate;
	
	void Awake() {
		  drawUI = GetComponent<MenuUI>().drawUI;
	}
	
	private void DrawSkillSelect() {
		if(GUI.Button(
			new Rect(Screen.width*0.20f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.05f), "ActiveSkill1")){}
		
		if(GUI.Button(
			new Rect(Screen.width*0.20f, Screen.height*0.75f, Screen.width*0.1f, Screen.height*0.05f), "Skill1"))
				skillDelegate += SetHeal;
		
		if(GUI.Button(
			new Rect(Screen.width*0.50f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.05f), "Done"))
				skillDelegate();			
	}

	public void SetSkillSelect() {
		GetComponent<MenuUI>().drawUI = DrawSkillSelect;
	}	
	
	private void SetHeal() {
		Debug.LogWarning("SetHeal");
		GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerData>().skills.skill1 = new Heal(GetComponent<PlayerStats>());
	}
}
