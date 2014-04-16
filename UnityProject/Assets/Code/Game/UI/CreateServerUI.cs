using UnityEngine;
using System.Collections;

public class CreateServerUI : MonoBehaviour {

    private MenuUI menuUI;
    private ServerGameUI serverGameUI;

    void Start() {
        menuUI = GetComponent<MenuUI>();
        serverGameUI = GetComponent<ServerGameUI>();
    }

    private void DrawCreateServer() {
        Rect screenCoordinates = new Rect(Screen.width * 0.5f - 50f, 30f, 100f, 20f);
        GameData.SERVER_NAME = GUI.TextField(screenCoordinates, GameData.SERVER_NAME);

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, 55f, 100f, 50f), "Create")) {
            ServerManager.instance.Register();
           
            serverGameUI.SetServerGameState();
            GameData.DRAW_CHAT = true;
            GameData.DRAW_STATS = true;
        }

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height - 70f, 100f, 50f), "Back")) {
            ServerManager.instance.miniBurzokGrounds.SetActive(false);
            ServerManager.instance.miniCrystalCaverns.SetActive(false);
            menuUI.SetMainMenuState();
        }

        GUI.Box(
            new Rect(Screen.width * 0.05f, Screen.height * 0.2f, Screen.width * 0.3f, Screen.height * 0.2f), "Select Map");
        if (GUI.Button(new Rect(
            Screen.width * 0.07f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.1f), "Death Match")) {
            ServerManager.instance.selectedMap = Map.DEATH_MATCH;

            ServerManager.instance.miniCrystalCaverns.SetActive(true);
            ServerManager.instance.miniBurzokGrounds.SetActive(false);
        }
        if (GUI.Button(new Rect(
            Screen.width * 0.23f, Screen.height * 0.25f, Screen.width * 0.1f, Screen.height * 0.1f), "Burzok Grounds"))  {
            ServerManager.instance.selectedMap = Map.BurzokGrounds;

            ServerManager.instance.miniBurzokGrounds.SetActive(true);
            ServerManager.instance.miniCrystalCaverns.SetActive(false);
        }
    }

    public void SetCreateServerState() {
        DrawUI.instance.drawUI = DrawCreateServer;
    }
}
