using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {

	private string currentMessage ;
	private string playerName = "";
	private UITextList chatHistory;
	private UIInput uiInput;

	private void Start()
	{
		chatHistory = GameObject.Find("Chat Window").transform.FindChild("Chat Area").GetComponent<UITextList>();
		uiInput = gameObject.GetComponent<UIInput>();
		uiInput.label.maxLineCount = 1;
	}

	public void SubmitInput()
	{
		string currentMessage = NGUIText.StripSymbols(uiInput.value);
			
		if (!string.IsNullOrEmpty(currentMessage))
			{
				GetPlayerChatInfo();
				playerName = playerName + ": ";

				currentMessage = playerName + currentMessage;
				if (currentMessage!=playerName)
					networkView.RPC("sendChatMessageToServer",RPCMode.Server,currentMessage);

				uiInput.value = "";
				uiInput.isSelected = false;
			}
	}

	[RPC]
	void sendChatMessageToServer(string message)
	{
		chatHistory.Add(message);
		networkView.RPC("sendChatMessageToClient",RPCMode.Others,message);
	}
	
	[RPC]
	void sendChatMessageToClient(string message)
	{
		chatHistory.Add(message);
	}	
	
	void GetPlayerChatInfo()
	{
		NetworkViewID id = GetComponent<Networking>().getMyPlayerID();
		List<PlayerData> playerList = GetComponent<PlayerList>().playerList;
		PlayerData player = playerList.Find(playerToFind => playerToFind.id == id);
			
		playerName = player.playerName;		
	}	
}