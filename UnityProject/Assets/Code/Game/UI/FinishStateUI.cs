using UnityEngine;
using System.Collections;

public class FinishStateUI : MonoBehaviour {

    private Rotator myPlayerRotator;

    private void DrawWinState() {
        GUI.Box(new Rect(Screen.width * 0.5f - 100f, 50f, 200f, 80f), "You Win");
        Input.ResetInputAxes();

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height * 0.7f, 100f, 30f), "Disconnect")) {
            Network.Disconnect();
            Application.LoadLevel(0);
            Destroy(gameObject);
        }
    }

    private void DrawLoseState() {
        GUI.Box(new Rect(Screen.width * 0.5f - 100f, 50f, 200f, 80f), "You Lose");
        Input.ResetInputAxes();


        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height * 0.7f, 100f, 30f), "Disconnect")) {
            Network.Disconnect();
            Application.LoadLevel(0);
            Destroy(gameObject);
        }
    }

    public void SetWinState() {
		GameData.instance.gameDataAsset.DRAW_CHAT = false;
		GameData.instance.gameDataAsset.DRAW_STATS = false;
		GameData.instance.gameDataAsset.DRAW_MINIMAP = false;

        if (Network.isClient) {
            myPlayerRotator = PlayerManager.instance.myPlayer.GetComponent<Rotator>();
            myPlayerRotator.enabled = false;
            Screen.lockCursor = false;
        }

        DrawUI.instance.drawUI = DrawWinState;
    }

    public void SetLoseState() {
		GameData.instance.gameDataAsset.DRAW_CHAT = false;
		GameData.instance.gameDataAsset.DRAW_STATS = false;
		GameData.instance.gameDataAsset.DRAW_MINIMAP = false;
        Screen.lockCursor = false;

        if (Network.isClient) {
            myPlayerRotator = PlayerManager.instance.myPlayer.GetComponent<Rotator>();
            myPlayerRotator.enabled = false;
            Screen.lockCursor = false;
        }

        DrawUI.instance.drawUI = DrawLoseState;
    }
}
