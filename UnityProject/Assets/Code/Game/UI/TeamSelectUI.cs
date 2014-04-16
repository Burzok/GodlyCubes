using UnityEngine;
using System.Collections;

public class TeamSelectUI : MonoBehaviour {
   
    private Networking networking;

    private MenuUI menuUI;
    private ClientGameUI clientGameUI;

    void Start() {
        networking = GetComponent<Networking>();
        menuUI = GetComponent<MenuUI>();
        clientGameUI = GetComponent<ClientGameUI>();
    }

	private void DrawTeamSelect()
    {
        if (GUI.Button(new Rect(Screen.width * 0.5f - 120f, 20f, 100f, 50f), "Team A")) {
            networking.ConnectToGame(Team.TEAM_A);
            GameData.DRAW_CHAT = true;
            GameData.DRAW_STATS = true;
            GameData.DRAW_MINIMAP = true;
			clientGameUI.SetClientGameState();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f + 20f, 20f, 100f, 50f), "Team B")) {
            networking.ConnectToGame(Team.TEAM_B);
            clientGameUI.SetClientGameState();
            GameData.DRAW_CHAT = true;
            GameData.DRAW_STATS = true;
            GameData.DRAW_MINIMAP = true;
			clientGameUI.SetClientGameState();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height - 70f, 100f, 50f), "Disconnect")) {
            Network.Disconnect();
            menuUI.SetMainMenuState();
        }
    }

    public void SetTeamSelectState() {
        DrawUI.instance.drawUI = DrawTeamSelect;
    }
}
