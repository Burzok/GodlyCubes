using UnityEngine;
using System.Collections;

public class ClientConnectUI : MonoBehaviour
{
	private GameObject mainMenuUI;
	private GameObject connectServerUI;

	[SerializeField]
	private GameObject serverEntry;
	[SerializeField]
	private UIInput playerName;
	private GameObject localServerEntry;
	private GameObject serverTable;

	private void Awake() 
	{
		mainMenuUI = GameObject.Find("MainMenu");
		connectServerUI = GameObject.Find("ConnectServer");
		serverTable = connectServerUI.transform.FindChild("ServerList").FindChild("Table").gameObject;
	}

	private void Start() 
	{
		playerName.value = GameData.PLAYER_NAME;
	}

	public void ServerListUpdate() {
		serverTable.GetComponent<UITable>().Reposition();

        ServerManager.instance.RequestServersList();
        HostData[] data = MasterServer.PollHostList();
        foreach (HostData element in data) {
            string name = element.gameName + " - " + (element.connectedPlayers - 1) + " / " + (element.playerLimit);

            string hostInfo;
            hostInfo = " - [";
            foreach (var host in element.ip) {
                hostInfo = hostInfo + host;
            }
            hostInfo = hostInfo + "]";
			localServerEntry = NGUITools.AddChild(serverTable, serverEntry);
			localServerEntry.transform.GetChild(1).GetComponent<ServerLabelIPContainer>().server = element;
			localServerEntry.transform.GetChild(0).GetComponent<UILabel>().text = name + hostInfo;
        }
    }

	public void SavePlayerName()
	{
		GameData.PLAYER_NAME = playerName.value;
	}

	public void Back()
	{
		GameObject[] serverEntries = GameObject.FindGameObjectsWithTag(Tags.serverEntry);
		foreach (GameObject entry in serverEntries)
		{
			Destroy(entry);
		}
		mainMenuUI.SetActive(true);
		connectServerUI.SetActive(false);
	}
	
	public void Reload()
	{
		GameObject[] serverEntries = GameObject.FindGameObjectsWithTag(Tags.serverEntry);
		foreach (GameObject entry in serverEntries)
		{
			Destroy(entry);
		}
		ServerListUpdate();
	}
}
