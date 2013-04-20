using UnityEngine;
using System.Collections;

public class ChatClient : MonoBehaviour {
	
	bool typing = false;
	bool getinfo = false;
	string currentmessage = "";
	string playername = "";
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Return))
		{
			typing = !typing;
		}
	}
	
	void OnGUI () {		
		CheckChatField();	
		
	}
	
	void CheckChatField() {
		if(typing)
		{
			networkView.RPC("GetPlayerInfo",RPCMode.Server, networkView.viewID);
			if(Event.current.Equals( Event.KeyboardEvent("return")))
			{
				currentmessage = playername + currentmessage;
				//chathistory.Add (currentmessage);
				typing = !typing;
				currentmessage = "";
			}
			GUI.SetNextControlName("Chat_Entry");
			currentmessage = GUI.TextField(new Rect(5f, Screen.height-30f, 200, 20), currentmessage); 
			if (currentmessage == string.Empty) {
				GUI.FocusControl("Chat_Entry");
			}
			
		}	
	}
	
	[RPC]
	void InfoToClient(string playerinfo)
	{
		Debug.Log ("jestem");
		playername=playerinfo;
	}
}
