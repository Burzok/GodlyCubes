using UnityEngine;

public class SkillSelectUI : MonoBehaviour {

    [SerializeField]
    private int keyToBind;
    [SerializeField]
    private Texture healthTexture;
    [SerializeField]
    private Texture attackTexture;

    private Networking networking;
    private ClientGameUI clientGameUI;
    private TeamSelectUI teamSecectUI;
    private Rect selectRect;
	
	void Awake() {
        networking = GetComponent<Networking>();
        teamSecectUI = GetComponent<TeamSelectUI>();
        clientGameUI = GetComponent<ClientGameUI>();
		keyToBind = 1;
	}
	
	private void DrawSkillSelect() {

        if (keyToBind == 1)
            selectRect = new Rect(Screen.width * 0.05f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.3f);
        else if (keyToBind == 2)
            selectRect = new Rect(Screen.width * 0.25f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.3f);
        else if (keyToBind == 3)
            selectRect = new Rect(Screen.width * 0.45f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.3f);
        else if (keyToBind == 4)
            selectRect = new Rect(Screen.width * 0.65f, Screen.height * 0.15f, Screen.width * 0.2f, Screen.height * 0.3f);
        else if (keyToBind == 5)
            selectRect = new Rect(-500, -500, 5, 5);
        
        GUI.Box(selectRect, GUIContent.none);
       
        if (GUI.Button(
            new Rect(Screen.width * 0.10f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.15f), "1")) 
        {
			SkillSelect.instance.removeSkill(1);
			keyToBind = 1;
		}
        if (GUI.Button(
            new Rect(Screen.width * 0.30f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.15f), "2"))
        {
            SkillSelect.instance.removeSkill(2);
            keyToBind = 2;
        }
        if (GUI.Button(
            new Rect(Screen.width * 0.50f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.15f), "3"))
        {
            SkillSelect.instance.removeSkill(3);
            keyToBind = 3;
        }
        if (GUI.Button(
            new Rect(Screen.width * 0.70f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.15f), "4"))
        {
            SkillSelect.instance.removeSkill(4);
            keyToBind = 4;
        }

        if (GUI.Button(
            new Rect(Screen.width * 0.20f, Screen.height * 0.75f, Screen.width * 0.1f, Screen.height * 0.15f), 
            healthTexture)) 
        {
            Debug.LogWarning(SkillSelect.instance);
			SkillSelect.instance.AddSkill(Skills.HEAL, keyToBind);
			keyToBind++;
		}
        if (GUI.Button(
            new Rect(Screen.width * 0.30f, Screen.height * 0.75f, Screen.width * 0.1f, Screen.height * 0.15f),
            attackTexture))
        {
            Debug.LogWarning(SkillSelect.instance);
            SkillSelect.instance.AddSkill(Skills.HEAL, keyToBind);
            keyToBind++;
        }

        if (GUI.Button(
            new Rect(Screen.width * 0.80f, Screen.height * 0.50f, Screen.width * 0.1f, Screen.height * 0.15f), "Done")) 
        {
            clientGameUI.SetClientGameState();
            networking.ConnectToGame(teamSecectUI.GetSelectedTeam() );
			SkillSelect.instance.skillsToChose();
		}
	}

	public void SetSkillSelect() {
        DrawUI.instance.drawUI = DrawSkillSelect;
	}
}
