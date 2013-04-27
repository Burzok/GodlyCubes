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
<<<<<<< HEAD:UnityProject/Assets/Scripts/Networking/Chat.cs
			if (Input.GetKeyUp(KeyCode.Return)) {
				networkView.RPC("GetPlayerInfo", RPCMode.Server, gameObject.GetComponent<Networking>().getPlayerID());
=======
			if (Input.GetKeyUp(KeyCode.Return))	{
				GetPlayerChatInfo();
>>>>>>> origin/master:UnityProject/Assets/Scripts/Chat.cs
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
<<<<<<< HEAD:UnityProject/Assets/Scripts/Networking/Chat.cs
			if(Event.current.Equals( Event.KeyboardEvent("return"))) {
=======
			if(Event.current.Equals( Event.KeyboardEvent("return"))) {				
				playerName = playerName + ": ";
>>>>>>> origin/master:UnityProject/Assets/Scripts/Chat.cs
				currentMessage = playerName + currentMessage;
				
				if (currentMessage!=playerName)
					networkView.RPC("sendChatMessageToServer",RPCMode.Server,currentMessage);
				
				typing = !typing;
				currentMessage = "";
			}
<<<<<<< HEAD:UnityProject/Assets/Scripts/Networking/Chat.cs
			
=======
>>>>>>> origin/master:UnityProject/Assets/Scripts/Chat.cs
			if(Event.current.Equals (Event.KeyboardEvent ("escape"))) {
				currentMessage = "";
				typing = !typing;
			}
			
			GUI.SetNextControlName("Chat_Entry");
			currentMessage = GUI.TextField(new Rect(5f, Screen.height-30f, 300f, 20f), currentMessage); 
<<<<<<< HEAD:UnityProject/Assets/Scripts/Networking/Chat.cs
			
			if (currentMessage == string.Empty) {
				GUI.FocusControl("Chat_Entry");
			}			
		}	
	}
	
	void DrawChat() {
		if (Network.isClient || Network.isServer) {
			GUI.Box (new Rect(5f, Screen.height-200f, 300, 165f),"");
			GUI.BeginGroup(new Rect(5f, Screen.height-200f, 300, 165f));
			scroll = GUILayout.BeginScrollView(scroll,GUILayout.Width(300),GUILayout.Height(165));	
=======
			if (currentMessage == string.Empty) 
				GUI.FocusControl("Chat_Entry");		
		}	
	}
	
	void DrawChat()	{
		if (Network.isClient || Network.isServer) {
			GUI.Box (new Rect(5f, Screen.height-200f, 300f, 165f),"");
			GUI.BeginGroup(new Rect(5f, Screen.height-200f, 300, 165f));
			scroll = GUILayout.BeginScrollView(scroll,GUILayout.Width(300),GUILayout.Height(165));			
>>>>>>> origin/master:UnityProject/Assets/Scripts/Chat.cs
			
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
<<<<<<< HEAD:UnityProject/Assets/Scripts/Networking/Chat.cs
	}
	
	[RPC] //Client function
	void InfoToClient(string playerInfo, Vector3 color) {
		playerName=playerInfo;
	}
	
	[RPC] //Server & Client function
	void UpdatePlayer(NetworkViewID id, Vector3 color) {
		GameObject []players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			if (id == player.networkView.viewID) {					
				player.GetComponentInChildren<Renderer>().material.color = new Color(color[0],color[1],color[2]);
			}
		}
	}
=======
	}	
	
	void GetPlayerChatInfo() {
		NetworkViewID id = GetComponent<Networking>().getPlayerID();
		List<PlayerData> playerList = GetComponent<PlayerList>().playerList;
		PlayerData player = playerList.Find(playerToFind => playerToFind.id == id);
			
		playerName=player.name;		
	}	
>>>>>>> origin/master:UnityProject/Assets/Scripts/Chat.cs
}
