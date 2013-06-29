using UnityEngine;
using System.Collections;

public abstract class ActiveSkill : Skill {
	
	protected float cooldown;
	protected KeyCode activationKey;
	
	[RPC]
	protected void SetCooldown(float time) {
	}
	
	[RPC]
	protected void ApplyStun(float time, NetworkViewID target) {
		
	}
	
	[RPC]
	protected void ApplyBlind(float time, NetworkViewID target) {
		
	}
	
	[RPC]
	protected void ApplySlow(float time, float percentage, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplyImmoblize(float time, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplyTaunt(float time, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplyFear(float time, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplyDamage(float damage, NetworkViewID target) {		
	}
	
	[RPC]
	protected void ApplyDamageOverTime(float damage, float time, NetworkViewID target) {		
	}
	
	[RPC]
	protected void ApplyHeal(float health, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplyHealOverTime(float health, float time, NetworkViewID target) {
	}
	
	[RPC]
	protected void ApplySpeedBoost(float percentage, NetworkViewID target) {
		
	}	
	
}
