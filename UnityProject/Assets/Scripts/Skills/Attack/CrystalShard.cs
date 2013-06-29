using UnityEngine;
using System.Collections;

public class CrystalShard : ActiveSkill {
	
	private void Start() {
		name = "Crystal Shard";
		power = 15.0f;
		range = 20.0f;
		radio = 0.0f;
		cooldown = 10.0f;
		duration = 2.0f;
		type =  SkillType.Single;
		owner = networkView.viewID;
	}
	
	void OnTriggerEnter(Collider other) {
		if(Network.isServer) {
			networkView.RPC("ApplyStun", RPCMode.All, other.networkView, duration);
			networkView.RPC("ApplyDamage", RPCMode.All, other.networkView, power);
		
			Network.Destroy(this.gameObject);
		}
	}
		
	override public string DescriptionString() {
		return "Crystal Shards fly toward target, stunning for a while and dealing damage.";
	}
}
