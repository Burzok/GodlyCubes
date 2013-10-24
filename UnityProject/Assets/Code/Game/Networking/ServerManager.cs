using UnityEngine;
using System.Collections.Generic;

public class ServerManager : SingletonComponent<ServerManager> {

    private string serverName = "GodlyCubes";
    public GameObject miniCrystalCaverns;
    public GameObject miniBurzokGrounds;
    public Map selectedMap;
    public float timeHostWasRegistered;
    public List<PlayerData> playerList;

    private int lastLevelPrefix;

    void Start() {
        playerList = GetComponent<PlayerList>().playerList;
        selectedMap = Map.CRYSTAL_CAVERNS;
    }
    
    public void Register() {
        lastLevelPrefix = 0;

        Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(serverName, GameData.SERVER_NAME);
        timeHostWasRegistered = Time.time;

        Network.SetLevelPrefix(lastLevelPrefix + 1);
        Application.LoadLevel(GameData.CURRENT_LEVEL_NAME + "Server");
    }

    public void Unregister() {
        Network.Disconnect();
        Application.LoadLevel(0);
        Destroy(this);
    }

    public void RequestServersList() {
        if (Time.time - timeHostWasRegistered >= 1.0f) {
            MasterServer.RequestHostList("GodlyCubesLight");
        }
    }

    void OnLevelWasLoaded(int level) {

        if (level == GameData.LEVEL_CRYSTAL_CAVERNS_CLIENT) {
            foreach (NetworkPlayer player in Network.connections) {
                Network.SetReceivingEnabled(player, 0, true);
            }
            Network.SetSendingEnabled(0, true);
        }

        if (level == GameData.LEVEL_MAIN_MENU) {
            miniCrystalCaverns = GameObject.Find("miniCrystalCaverns");
            miniBurzokGrounds = GameObject.Find("miniBurzokGrounds");

            miniCrystalCaverns.SetActive(false);
            miniBurzokGrounds.SetActive(false);
        }
    }

    private void InitializeConnecting() {
        networkView.RPC("PauseGame", RPCMode.All);
        networkView.RPC("IncPlayersConnectingNumber", RPCMode.Others);
        networkView.RPC("ShowPlayersConnectingPopup", RPCMode.Others);
        networkView.RPC("GetLevelFromServer", RPCMode.Server, Network.player);
        DrawUI.instance.drawUI -= InitializeConnecting;
    }

    [RPC]
    private void PauseGame() {
        if (GameTime.isPaused == false)
            GameTime.PauseGame();
    }

    [RPC]
    private void IncPlayersConnectingNumber() {
        GameData.NUMBER_OF_CONNECTING_PLAYERS++;
    }

    [RPC]
    private void ShowPlayersConnectingPopup() {
        if (DrawUI.instance.drawUI != DrawPlayerConnectingPopup)
            DrawUI.instance.drawUI += DrawPlayerConnectingPopup;
    }

    [RPC]
    private void GetLevelFromServer(NetworkPlayer player) {
        networkView.RPC("LoadLevelOnClient", player, GameData.CURRENT_LEVEL_NAME, lastLevelPrefix + 1);
    }

    [RPC]
    private void LoadLevelOnClient(string level, int levelPrefix) {
        lastLevelPrefix = levelPrefix;
        Network.SetLevelPrefix(levelPrefix);

        if (Network.isServer)
            Application.LoadLevel(level + "Server");
        else
            Application.LoadLevel(level + "Client");
    }

    private void DrawPlayerConnectingPopup() {
        for (int i = 0; i < GameData.NUMBER_OF_CONNECTING_PLAYERS; i++)
        {
            GUI.Box(new Rect(Screen.width * 0.5f - 200f, Screen.height * 0.3f + i * 20f, 400f, 20f),
                "NEW PLAYER CONNECTING, PLEASE WAIT");
        }
    }

    public void HidePlayersConnectingPopup() {
        if (DrawUI.instance.drawUI == DrawPlayerConnectingPopup)
            DrawUI.instance.drawUI -= DrawPlayerConnectingPopup;
    }

    public void AddInitializeConnecting() {
        DrawUI.instance.drawUI += InitializeConnecting;
    }

    void OnDisconnectedFromServer() {
        Application.LoadLevel(GameData.LEVEL_DISCONNECTED);
    }
}