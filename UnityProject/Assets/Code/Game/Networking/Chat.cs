using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {

	private string currentMessage ;
	private string playerName = "";
	private UITextList chatHistory;
	private UIInput uiInput;

	private Networking networking;
	private PlayerList playerListComponent;

	private void Awake() 
	{
		networking = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Networking>();
		playerListComponent = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>();
	}

	private void Start()
	{
		chatHistory = GameObject.Find("Chat Window").transform.FindChild("Chat Area").GetComponent<UITextList>();
		uiInput = gameObject.GetComponent<UIInput>();
		uiInput.label.maxLineCount = 1;
	}

	public void SubmitInput()
	{
		if(Network.isClient)
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
		NetworkViewID id = networking.getMyPlayerID();
		List<PlayerData> playerList = playerListComponent.playerList;
		PlayerData player = playerList.Find(playerToFind => playerToFind.id == id);
			
		playerName = player.playerName;		
	}	
}