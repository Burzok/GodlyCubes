using UnityEngine;

public class Heal : ScriptableObject, ISkill {
	private PlayerStats stats;
	private bool inUse;
	private float timer;
	private float cooldown;
	
	void Update() {
		if(inUse == true) {
			timer += Time.deltaTime;
			if(timer > cooldown) {
				inUse = false;
				timer = 0;
			}
		}
	}
	
	public void Use() {
		Debug.LogWarning("HEAL");
		inUse = true;
		timer = 0;
		stats.currentHealth = stats.maxHealth;
		SkillControler.singleton.skillInUse = false;
		inUse = false;
	}
	
	public bool InUse() {
		return inUse;
	}
	
	public void SetData(PlayerStats stats) {
		this.stats = stats;
		this.cooldown = Skills.HEAL_COOLDOWN;
		inUse = false;
		timer = 0;
	}
}
