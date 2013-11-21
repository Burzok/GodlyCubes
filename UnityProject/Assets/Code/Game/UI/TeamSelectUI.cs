using UnityEngine;
using System.Collections;

public class TeamSelectUI : MonoBehaviour {
   
    private SkillSelectUI skillSelectUI;
    private MenuUI menuUI;
    private Team selectedTeam;

    void Start() {
        
        skillSelectUI = GetComponent<SkillSelectUI>();
        menuUI = GetComponent<MenuUI>();
    }

    private void DrawTeamSelect()
    {
        if (GUI.Button(new Rect(Screen.width * 0.5f - 120f, 20f, 100f, 50f), "Team A")) {
            selectedTeam = Team.TEAM_A;
            skillSelectUI.SetSkillSelect();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f + 20f, 20f, 100f, 50f), "Team B")) {
            selectedTeam = Team.TEAM_B;
            skillSelectUI.SetSkillSelect();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height - 70f, 100f, 50f), "Disconnect")) {
            Network.Disconnect();
            menuUI.SetMainMenuState();
        }
    }

    public void SetTeamSelectState() {
        DrawUI.instance.drawUI = DrawTeamSelect;
    }

    public Team GetSelectedTeam() {
        return selectedTeam;
    }
}
