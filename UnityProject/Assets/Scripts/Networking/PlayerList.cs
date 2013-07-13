using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour {
	public Transform playerDataPrefab;
	public List<PlayerData> playerList = new List<PlayerData>(4);
	
	private MenuGUI menuGUI;
	private Networking networkingScript;
	
	void Awake() {
		menuGUI = GetComponent<MenuGUI>();
		networkingScript = GetComponent<Networking>();
	}	

	[RPC] // Clients & Server
	void RegisterPlayer(string playerName, NetworkViewID playerID, int playerTeam) {
		Transform player2 = Instantiate(playerDataPrefab, transform.position, transform.rotation) as Transform;

		player2.transform.parent = transform;
		
		PlayerData player = player2.GetComponent<PlayerData>();
		
		FillPlayerData(ref player, ref playerName, ref playerID, ref playerTeam);
		
		playerList.Add(player);
		
		UpdatePlayerLocalTransform(ref player.localTransform);
	}
	
	private void FillPlayerData(ref PlayerData player, ref string playerName, ref NetworkViewID playerID, ref int playerTeam) {
		player.id = playerID;
		player.name = playerName;
		player.team = (Team)playerTeam;
		
		GameObject[] localPlayers = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach (GameObject localPlayer in localPlayers) {
			if (player.id == localPlayer.networkView.viewID)
				player.localTransform = localPlayer.transform;
		}
		
		player.localTransform.GetComponent<ControllerBasic>().data = player;
		
		if (player.team == Team.TeamA) {
			player.color = new Vector3(1,0,0);
			player.name = "A"+networkingScript.numOfPlayersA;
		}
		else if (player.team == Team.TeamB) {
			player.color = new Vector3(0,0,1);
			player.name = "B"+networkingScript.numOfPlayersB;
		}
		
		player.respawnPosition = player.localTransform.position;
		
		player.kills=0;
		player.deaths=0;
		player.assist=0;		
	}
	
	private void UpdatePlayerLocalTransform(ref Transform localTransform) {
		ChangeColor(ref localTransform);
		ChangeLayers(ref localTransform);
	}
	
	private void ChangeColor(ref Transform localTransform) {
		PlayerData data = localTransform.GetComponent<ControllerBasic>().data;
		Transform playerArmature = localTransform.Find("Animator");
		Renderer[] playerRenderers = playerArmature.GetComponentsInChildren<Renderer>();
		float tuning = 0.5f;
		
		foreach(Renderer rend in playerRenderers) {
			if(rend.gameObject.name == "Player")
				rend.material.color = new Color(data.color.x*tuning, data.color.y*tuning, data.color.z*tuning);
			else 
				rend.material.color = new Color(data.color.x, data.color.y, data.color.z);
		}
	}
	
	private void ChangeLayers(ref Transform localTransform) {
		PlayerData data = localTransform.GetComponent<ControllerBasic>().data;
		
		if (data.team == Team.TeamA) 
			localTransform.gameObject.layer=13;
		else if (data.team == Team.TeamB)
			localTransform.gameObject.layer=14;			
	}
		
	[RPC] //Client function
	private void InfoToClient(NetworkViewID id, string playerName, Vector3 color, int team, int kills, int deaths, int assist) {
		PlayerData player = new PlayerData();
		FillPlayerData(ref player, id, playerName, color, team, kills, deaths, assist);
		playerList.Add(player);
	}
	
	private void FillPlayerData(ref PlayerData player, NetworkViewID id, string playerName, Vector3 color, int team, int kills, int deaths, int assist) {
		player.id=id;
		player.name=playerName;
		player.color=color;
		player.team=(Team)team;
		player.kills=kills;
		player.deaths=deaths;
		player.assist=assist;
		
		GameObject[] localPlayers = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach (GameObject localPlayer in localPlayers) {
			if (player.id == localPlayer.networkView.viewID)
				player.localTransform = localPlayer.transform;
		}
	}
	
	private void SynchronizeClient(NetworkViewID clientID) {
		foreach(PlayerData data in playerList) {
			networkView.RPC("SynchronizeList", clientID.owner, data.id, data.kills, data.deaths, data.assist);
		}
	}
	
	[RPC]
	private void SynchronizeList(NetworkViewID id, int kills, int deaths, int assist) {
		int ind = playerList.FindIndex(player => player.id == id);
		playerList[ind].kills=kills;
		playerList[ind].deaths=deaths;
		playerList[ind].assist=assist;
	}
	
	[RPC] //Server function
	private void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);		
	}
	
	[RPC] //Server function
	private void GetPlayerInfo(NetworkViewID idToFind) { 
		
		PlayerData playerToSend = new PlayerData();
		playerToSend = playerList.Find(playerToFind => playerToFind.id == idToFind);
		
		networkView.RPC("InfoToClient", idToFind.owner, idToFind.owner, 
						playerToSend.name, playerToSend.color, playerToSend.kills, 
						playerToSend.deaths, playerToSend.assist);
	}
	
	void OnGUI() {
		if (menuGUI.drawStats) { 
			GUI.Box (new Rect(5f, 50f, 140f, 50f),"");
			GUI.BeginGroup(new Rect(5f, 50f, 140f, 50f));
			foreach(PlayerData player in playerList) {
				GUILayout.BeginHorizontal();
				
	        	if (player.color == new Vector3(1, 0, 0))  
	          		GUI.contentColor = Color.red;
	       		if (player.color == new Vector3(0, 0, 1))
	          		GUI.contentColor = Color.blue;
	       
				GUILayout.Space(5f);				
				GUILayout.Label(player.playerName+": ");
				
				GUI.contentColor = Color.green;
				GUILayout.Label("K: "+player.kills);
				GUILayout.FlexibleSpace();
				
				GUI.contentColor = Color.red;
				GUILayout.Label("D: "+player.deaths);
				GUILayout.FlexibleSpace();
				
				GUI.contentColor = Color.yellow;
				GUILayout.Label("A: "+player.assist);
				GUILayout.FlexibleSpace();
				
				GUILayout.EndHorizontal();
			}
			GUI.EndGroup();
		}
	}
}
