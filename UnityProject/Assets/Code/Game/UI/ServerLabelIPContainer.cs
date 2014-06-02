using UnityEngine;
using System.Collections;

public class ServerLabelIPContainer : MonoBehaviour {

	public HostData server;
	
	private UIInput playerName;

	private void Awake() 
	{
		playerName = GameObject.Find("Name Input").GetComponent<UIInput>();
	}

	public void Connect()
	{		
		GameData.SERVER_NAME = server.gameName;
		GameData.PLAYER_NAME = playerName.value;
		Network.Connect(server);	
	}
}
