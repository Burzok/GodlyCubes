using UnityEngine;

public class SkillSelectUI : MonoBehaviour {
	
	public int keyToBind;
	
	void Awake() {
		keyToBind = 1;
	}
	
	private void DrawSkillSelect() {
		if(GUI.Button(
			new Rect(Screen.width*0.20f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.05f), "ActiveSkill1"))
		{
			SkillSelect.singleton.removeSkill(keyToBind);
			keyToBind--;
		}
		
		if(GUI.Button(
			new Rect(Screen.width*0.20f, Screen.height*0.75f, Screen.width*0.1f, Screen.height*0.05f), Skills.HEAL))
		{
			SkillSelect.singleton.AddSkill(Skills.HEAL, keyToBind);
			keyToBind++;
		}
		
		if(GUI.Button(
			new Rect(Screen.width*0.50f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.05f), "Done"))
		{
			SkillSelect.singleton.skillsToChose();
			MenuUI.singleton.SetClientGameState();
		}
	}

	public void SetSkillSelect() {
		DrawUI.singleton.drawUI = DrawSkillSelect;
	}
}
