using UnityEngine;
using System.Collections;

public class BaseTeamChoserClient : MonoBehaviour { // TODO: add color change
	public Team teamSelect;

	private Transform baseChild;

	void Awake() {
		baseChild = transform.GetChild(0).GetChild(0);
	}

	public void SetTeam(ref Team team) {
		teamSelect = team;
		
		if (teamSelect == Team.TEAM_A) {
			gameObject.layer = PhysicsLayers.LAYER_TEAM_A;
			baseChild.transform.GetChild(0).renderer.material.color = Color.red;
			baseChild.transform.GetChild(1).renderer.material.color = Color.red;
		}
		else if (teamSelect == Team.TEAM_B) {
			gameObject.layer = PhysicsLayers.LAYER_TEAM_B;
			baseChild.transform.GetChild(0).renderer.material.color = Color.blue;
			baseChild.transform.GetChild(1).renderer.material.color = Color.blue;

		}
	}
}
