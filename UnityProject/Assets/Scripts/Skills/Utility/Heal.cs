using UnityEngine;

public class Heal : ISkill {
	PlayerStats stats;
	public void Use() {
			stats.currentHealth=stats.maxHealth;	
	}
	
	public Heal(PlayerStats stats) {
		this.stats = stats;
	}
}
