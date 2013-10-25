using UnityEngine;
using System.Collections;

public class ServerGameUI : MonoBehaviour {
    
    private void DrawServerGame() {
        GUI.Label(new Rect(5, 5, 250, 40), "Server name: " + GameData.SERVER_NAME);

        if (GUI.Button(new Rect(Screen.width - 105f, 5f, 100f, 50f), "Back")) {
            GameData.DRAW_CHAT = false;
            DrawUI.instance.drawUI = null;
            ServerManager.instance.Unregister();           
        }
    }

    public void SetServerGameState() {
        DrawUI.instance.drawUI = DrawServerGame;
    }
}
