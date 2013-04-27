using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {
	
	bool typing = false;
	string currentMessage = "";
	string playerName = "";
	Vector2 scroll;
	List<string> chatHistory = new List<string>();
	
	void Update () {
		if (Network.isClient) {
			if (Input.GetKeyUp(KeyCode.Return))	{
				GetPlayerChatInfo();
				typing = !typing;
			}
		}
	}
	
	void OnGUI () {
		CheckChatInput();
		DrawChat();
	}
	
	void CheckChatInput() {
		if(typing) {
			if(Event.current.Equals( Event.KeyboardEvent("return"))) {				
				playerName = playerName + ": ";
				currentMessage = playerName + currentMessage;
				
				if (currentMessage!=playerName)
					networkView.RPC("sendChatMessageToServer",RPCMode.Server,currentMessage);
				
				typing = !typing;
				currentMessage = "";
			}
			if(Event.current.Equals (Event.KeyboardEvent ("escape"))) {
				currentMessage = "";
				typing = !typing;
			}
			
			GUI.SetNextControlName("Chat_Entry");
			currentMessage = GUI.TextField(new Rect(5f, Screen.height-30f, 300f, 20f), currentMessage); 
			if (currentMessage == string.Empty) 
				GUI.FocusControl("Chat_Entry");		
		}	
	}
	
	void DrawChat()	{
		if (Network.isClient || Network.isServer) {
			GUI.Box (new Rect(5f, Screen.height-200f, 300f, 165f),"");
			GUI.BeginGroup(new Rect(5f, Screen.height-200f, 300, 165f));
			scroll = GUILayout.BeginScrollView(scroll,GUILayout.Width(300),GUILayout.Height(165));			
			
			foreach (string entry in chatHistory) {
				GUILayout.BeginHorizontal();
				GUILayout.Space(5f);				
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUILayout.Label(entry, GUILayout.Width(250));
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView(); 
			GUI.EndGroup();
		}
	}	

	
	[RPC] //Server function
	void sendChatMessageToServer(string message) {
		chatHistory.Add(message);
		scroll.y=10000000;
		networkView.RPC("sendChatMessageToClient",RPCMode.Others,message);
	}
	
	[RPC] //Client function
	void sendChatMessageToClient(string message) {
		chatHistory.Add(message);
		scroll.y=10000000;
	}	
	
	void GetPlayerChatInfo() {
		NetworkViewID id = GetComponent<Networking>().getPlayerID();
		List<PlayerData> playerList = GetComponent<PlayerList>().playerList;
		PlayerData player = playerList.Find(playerToFind => playerToFind.id == id);
			
		playerName=player.name;		
	}	
}
