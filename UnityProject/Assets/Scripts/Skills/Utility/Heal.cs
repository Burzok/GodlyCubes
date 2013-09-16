using UnityEngine;

public class Heal : ISkill {
	private PlayerStats stats;
	
	public Heal(PlayerStats stats) {
		this.stats = stats;
	}
	
	public void Use() {
		Debug.LogWarning("HEAL");
		stats.currentHealth = stats.maxHealth;	
	}
}
