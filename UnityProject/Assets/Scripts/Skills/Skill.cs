using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Skill : MonoBehaviour {
	
	List<PlayerData> list;
	
	public NetworkViewID owner;
	public NetworkViewID id;
	
	protected float power;
	protected float range;
	protected float radio;
	protected string type;
	protected float duration;
	
	protected void Awake() {
		id = Network.AllocateViewID();
		list = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerList>().playerList;
	}
	
	protected void CheckStatsUpdate() {
		
	}
	
	public abstract string DescriptionString();
}
