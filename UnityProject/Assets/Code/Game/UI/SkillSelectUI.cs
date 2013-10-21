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
			SkillSelect.instance.removeSkill(keyToBind);
			keyToBind--;
		}
		
		if(GUI.Button(
			new Rect(Screen.width*0.20f, Screen.height*0.75f, Screen.width*0.1f, Screen.height*0.05f), Skills.HEAL))
		{
            Debug.LogWarning(SkillSelect.instance);
			SkillSelect.instance.AddSkill(Skills.HEAL, keyToBind);
			keyToBind++;
		}
		
		if(GUI.Button(
			new Rect(Screen.width*0.50f, Screen.height*0.25f, Screen.width*0.1f, Screen.height*0.05f), "Done"))
		{
			SkillSelect.instance.skillsToChose();
			MenuUI.instance.SetClientGameState();
		}
	}

	public void SetSkillSelect() {
        DrawUI.instance.drawUI = DrawSkillSelect;
	}
}
