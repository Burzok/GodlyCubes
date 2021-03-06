using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour {
	public List<PlayerData> playerList = new List<PlayerData>(4);
	
	public void CallRegisterPlayer(NetworkViewID playerID, int playerTeam) {
		networkView.RPC("RegisterPlayer", RPCMode.All, GameData.PLAYER_NAME, playerID, playerTeam);	
	}
	
	[RPC] // Clients & Server
	void RegisterPlayer(string playerName, NetworkViewID playerID, int playerTeam) {
		GameObject player = FindPlayer(ref playerID);
		
		PlayerData playerData = player.GetComponent<PlayerData>();
		
		FillPlayerData(ref playerData, ref playerName, ref playerID, ref playerTeam);
		
		playerList.Add(playerData);
		
		UpdatePlayerLocalTransform(player.transform);
	}
	
	public GameObject FindPlayer(ref NetworkViewID playerID) {
		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.player);
		foreach(GameObject player in players) {
			if(player.networkView.viewID == playerID)
				return player;
		}
		Debug.LogError("No Player");
		return null;
	}
	
	private void FillPlayerData(
		ref PlayerData player, ref string playerName, ref NetworkViewID playerID, ref int playerTeam)
	{
		player.id = playerID;
		player.playerName = playerName;
		player.team = (Team)playerTeam;
		
		if (player.team == Team.TEAM_A)
			player.color = new Vector3(1,0,0);
		else if (player.team == Team.TEAM_B)
			player.color = new Vector3(0,0,1);
		
		player.respawnPosition = player.transform.position;
		
		player.kills=0;
		player.deaths=0;
		player.assist=0;		
	}
	
	private void UpdatePlayerLocalTransform(Transform localTransform) {
		ChangeColor(ref localTransform);
		ChangeLayers(ref localTransform);
	}
	
	private void ChangeColor(ref Transform localTransform) {
		if(!Network.isServer) {
			PlayerData data = localTransform.GetComponent<PlayerData>();
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
	}
	
	private void ChangeLayers(ref Transform localTransform) {
		PlayerData data = localTransform.GetComponent<PlayerData>();
		
		if (data.team == Team.TEAM_A) 
			localTransform.gameObject.layer=13;
		else if (data.team == Team.TEAM_B)
			localTransform.gameObject.layer=14;			
	}
	
	[RPC] //Server function
	private void UnregisterPlayer(NetworkViewID idToDelete)	{
		Network.RemoveRPCs(idToDelete);
		playerList.RemoveAll(playerToDelete => playerToDelete.id == idToDelete);		
	}
}
