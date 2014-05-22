using UnityEngine;
using System.Collections;

public class ClientGameUI : MonoBehaviour {

    private Networking networking;
	private MinimapPlayerList mapList;
    private DrawStatsUI drawStatsUI;

    void Start() {
        networking = GetComponent<Networking>();
		mapList = GetComponent<MinimapPlayerList>();
        drawStatsUI = GetComponent<DrawStatsUI>();
    }

    private void DrawClientGame() {
		GUI.Label(new Rect(5, 5, 250, 40), "Server name: " + GameData.instance.gameDataAsset.SERVER_NAME);

        if (GUI.Button(new Rect(Screen.width - 105f, 5f, 100f, 50f), "Back")) {
            networkView.RPC("UnregisterPlayer", RPCMode.Server, networking.getMyPlayerID());
            Network.Disconnect();



			Application.LoadLevel(GameData.instance.gameDataAsset.LEVEL_MAIN_MENU);
			GameData.instance.gameDataAsset.DRAW_CHAT = false;
			GameData.instance.gameDataAsset.DRAW_MINIMAP = false;
			mapList.minimapPlayerList.Clear();
        }

        drawStatsUI.DrawStats();
    }

    public void SetClientGameState() {

    }
}
