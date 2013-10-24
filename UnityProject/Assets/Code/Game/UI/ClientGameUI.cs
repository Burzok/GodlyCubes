using UnityEngine;
using System.Collections;

public class ClientGameUI : MonoBehaviour {

    private Networking networking;
    private MenuUI menuUI;
    private DrawStatsUI drawStatsUI;

    void Start() {
        networking = GetComponent<Networking>();
        menuUI = GetComponent<MenuUI>();
        drawStatsUI = GetComponent<DrawStatsUI>();
    }

    private void DrawClientGame() {
        GUI.Label(new Rect(5, 5, 250, 40), "Server name: " + GameData.SERVER_NAME);

        if (GUI.Button(new Rect(Screen.width - 105f, 5f, 100f, 50f), "Back")) {
            networkView.RPC("UnregisterPlayer", RPCMode.Server, networking.getMyPlayerID());
            networkView.RPC("DecCounters", RPCMode.AllBuffered, GameData.ACTUAL_CLIENT_TEAM);
            Network.Disconnect();

            menuUI.SetMainMenuState();

            Application.LoadLevel(GameData.LEVEL_MAIN_MENU);
            GameData.DRAW_CHAT = false;
        }

        drawStatsUI.DrawStats();
    }

    public void SetClientGameState() {
        DrawUI.instance.drawUI = DrawClientGame;
    }
}
