using UnityEngine;
using System.Collections;

public class EndUI : MonoBehaviour {

	[SerializeField]
	private GameObject playerTableWin;
	[SerializeField]
	private GameObject playerTableLose;

	private GameObject playerTable;

	[SerializeField]
	private GameObject playerEntry;

	[SerializeField]
	private GameObject loseEnd;
	[SerializeField]
	private GameObject winEnd;
	[SerializeField]
	private GameObject gameHud;

	private GameObject localPlayerEntry;
	private string playerText;

	private GameObject[] players;

	public void Start()
	{
		winEnd.SetActive(false);
		loseEnd.SetActive(false);
	}

	public void BlockInput()
	{
		Screen.lockCursor = false;
		players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players)
		{
			player.GetComponent<Rotator>().enabled = false;
			player.GetComponent<MovementBasic>().enabled = false;
			player.GetComponent<ShootingControllerClient>().enabled = false;
		}
	}

	public void WonState()
	{
		winEnd.SetActive(true);
		gameHud.SetActive(false);
		playerTable = playerTableWin;
		ShowStats();
		BlockInput();
	}

	public void LoseState()
	{
		loseEnd.SetActive(true);
		gameHud.SetActive(false);
		playerTable = playerTableLose;
		ShowStats();
		BlockInput();
	}

	private void ShowStats()
	{
		foreach (PlayerData player in PlayerManager.instance.playerDataList)
		{
			localPlayerEntry = NGUITools.AddChild(playerTable, playerEntry);

			playerText = player.playerName +": ";
			playerText+= "[00ff00]Kills: " + player.kills + " [000000]| ";
			playerText+= "[ff0000]Deaths: " + player.deaths;

			localPlayerEntry.transform.GetChild(0).GetComponent<UILabel>().text = playerText;
		}	
		playerTable.GetComponent<UIGrid>().Reposition();
	}

	public void Disconnect()
	{
		Network.Disconnect();			
		Application.LoadLevel(GameData.LEVEL_MAIN_MENU);
		Destroy(gameObject);
	}
}
