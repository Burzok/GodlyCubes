using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour {
	public int damage = 10;
	
	private bool resFlag = false;
	private float timer = 0;
	private PlayerData data;
	private Renderer[] playerRenderers;
	private GameObject gameController;
	
	void Awake() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController);
		data = GetComponent<PlayerData>();
		
		if(!Network.isServer) {
			Transform playerArmature = transform.Find("Animator");
			playerRenderers = playerArmature.GetComponentsInChildren<Renderer>();	
		}
	}

	void Update() {
		if(resFlag)
			Respawn();
	}
	
	void Respawn() {
		timer += Time.deltaTime;
		if(timer >= data.respawnTime ) {
			networkView.RPC("ChangePlayerState",RPCMode.AllBuffered, networkView.viewID);
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
	
			timer = 0;
			resFlag = false;
		}
	}
	
	[RPC]
	void SwichPlayerState(NetworkViewID viewID) {
		if(networkView.viewID == viewID) {
			data.isAlive = !data.isAlive;
			collider.enabled = !collider.enabled;
			GetComponent<MovementBasic>().enabled = !GetComponent<MovementBasic>().enabled;
			GetComponent<Shooting>().enabled = !GetComponent<Shooting>().enabled;
			foreach(Renderer rend in playerRenderers)
				rend.enabled = !rend.enabled;
		}
	}
	
	[RPC]
	void ChangePlayerState(NetworkViewID viewID) {
		if(networkView.viewID == viewID) {
			this.transform.position = data.respawnPosition;
			data.health = data.maxHealth;
		}
	}
	
	void Hit(object[] package) {
		data.health -= (int)package[0];
		if(data.health <= 0) {
			networkView.RPC("SwichPlayerState",RPCMode.AllBuffered, networkView.viewID);
			
			List<PlayerData> playerList = gameController.GetComponent<PlayerList>().playerList;
			int id = playerList.FindIndex(player => player.id == networkView.viewID);
			playerList[id].deaths++;			
			networkView.RPC ("UpdateDeath", RPCMode.Others, networkView.viewID);
			
			id = playerList.FindIndex(player => player.id == (NetworkViewID)package[1]);	
			if(id!= -1) {
				playerList[id].kills++;	
				networkView.RPC ("UpdateKill", RPCMode.Others, (NetworkViewID)package[1]);
			}	
			resFlag = true;
		}
		else
			networkView.RPC("SendHitConfirmationToClients", RPCMode.OthersBuffered, networkView.viewID, damage);
	}
	
	[RPC]
	void UpdateKill(NetworkViewID viewID) {
		List<PlayerData> playerList = gameController.GetComponent<PlayerList>().playerList;
		int id = playerList.FindIndex(player => player.id == viewID);
		playerList[id].kills++;		
	}
	
	[RPC]
	void UpdateDeath(NetworkViewID viewID) {
		List<PlayerData> playerList = gameController.GetComponent<PlayerList>().playerList;
		int id = playerList.FindIndex(player => player.id == viewID);
		playerList[id].deaths++;		
	}
	
	[RPC]
	void SendHitConfirmationToClients(NetworkViewID viewID, int damage) {
		if(networkView.viewID == viewID)
			data.health -= damage;
	}
}
