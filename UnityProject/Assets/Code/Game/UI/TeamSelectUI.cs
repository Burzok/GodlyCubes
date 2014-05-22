using UnityEngine;
using System.Collections;

public class TeamSelectUI : MonoBehaviour {
   
    private Networking networking;
	
    private ClientGameUI clientGameUI;

    void Start() {
        networking = GetComponent<Networking>();
        clientGameUI = GetComponent<ClientGameUI>();
    }

	private void DrawTeamSelect()
    {
        if (GUI.Button(new Rect(Screen.width * 0.5f - 120f, 20f, 100f, 50f), "Team A")) {
            networking.ConnectToGame(Team.TEAM_A);
			GameData.instance.gameDataAsset.DRAW_CHAT = true;
            GameData.instance.gameDataAsset.DRAW_STATS = true;
			GameData.instance.gameDataAsset.DRAW_MINIMAP = true;
			clientGameUI.SetClientGameState();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f + 20f, 20f, 100f, 50f), "Team B")) {
            networking.ConnectToGame(Team.TEAM_B);
            clientGameUI.SetClientGameState();
			GameData.instance.gameDataAsset.DRAW_CHAT = true;
			GameData.instance.gameDataAsset.DRAW_STATS = true;
			GameData.instance.gameDataAsset.DRAW_MINIMAP = true;
			clientGameUI.SetClientGameState();
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height - 70f, 100f, 50f), "Disconnect")) {
            Network.Disconnect();
        }
    }

    public void SetTeamSelectState() {
    }
}
