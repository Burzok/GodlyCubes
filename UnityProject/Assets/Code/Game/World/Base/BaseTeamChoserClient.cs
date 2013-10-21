using UnityEngine;
using System.Collections;

public class BaseTeamChoserClient : MonoBehaviour { // TODO: add color change
	public Team teamSelect;

	public void SetTeam(ref Team team) {
		teamSelect = team;
		
		if (teamSelect == Team.TEAM_A) 
			gameObject.layer = PhysicsLayers.LAYER_TEAM_A;
		else if (teamSelect == Team.TEAM_B) 
			gameObject.layer = PhysicsLayers.LAYER_TEAM_B;
	}
}
