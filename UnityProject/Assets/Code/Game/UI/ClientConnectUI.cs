using UnityEngine;
using System.Collections;

public class ClientConnectUI : MonoBehaviour
{
    private Rect windowRect = new Rect(Screen.width * .5f - 200f, 20f, 400f, 150f);
    MenuUI menuUI;
    ClientConnectingUI clientConnectingUI;

    void Start() {
        menuUI = GetComponent<MenuUI>();
        clientConnectingUI = GetComponent<ClientConnectingUI>();
    }

    private void DrawConnect() {
        windowRect = GUI.Window(0, windowRect, ServerList, "Server List");

        Rect screenCoordinates = new Rect(Screen.width * 0.5f - 50f, 175f, 100f, 20f);
		GameData.instance.gameDataAsset.PLAYER_NAME = GUI.TextField(screenCoordinates, GameData.instance.gameDataAsset.PLAYER_NAME);

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height - 70f, 100f, 50f), "Back"))
            menuUI.SetMainMenuState();
    }

    private void ServerList(int windowID) {
        ServerManager.instance.RequestServersList();

        HostData[] data = MasterServer.PollHostList();
        foreach (HostData element in data) {
            GUILayout.BeginHorizontal();
            string name = element.gameName + " " + (element.connectedPlayers - 1) + " / " + (element.playerLimit);
            GUILayout.Label(name);
            GUILayout.Space(5);

            string hostInfo;
            hostInfo = "[";
            foreach (var host in element.ip) {
                hostInfo = hostInfo + host;
            }
            hostInfo = hostInfo + "]";

            GUILayout.Label(hostInfo);
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Connect")) {
                Network.Connect(element);
				GameData.instance.gameDataAsset.SERVER_NAME = element.gameName;
                clientConnectingUI.SetConnectingState();
            }

            GUILayout.EndHorizontal();
        }
    }

    public void SetConnectState() {
        DrawUI.instance.drawUI = DrawConnect;
    }
}
