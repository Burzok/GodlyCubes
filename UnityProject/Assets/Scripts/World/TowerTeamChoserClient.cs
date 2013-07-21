using UnityEngine;
using System.Collections;

public class TowerTeamChoserClient : MonoBehaviour {
	public Team teamSelect;
	
	private string teamName;
	private Vector3 iconPosition;
	
	void Awake () {
		teamName = "TowerNeutral";
		iconPosition = transform.position + new Vector3(0f,5f,0f);
	}
	
	public void SetTeam(Team team) {
		teamSelect = team;
		if (teamSelect == Team.TeamA) {
				teamName = "TowerTeamA";
				gameObject.layer = 13;
				transform.GetChild(0).gameObject.layer = 9;
				GetComponent<Reloader>().SetTeam(Team.TeamA);
			}
			else if (teamSelect == Team.TeamB) {
				teamName = "TowerTeamB";
				gameObject.layer = 14;
				transform.GetChild(0).gameObject.layer = 10;
				GetComponent<Reloader>().SetTeam(Team.TeamB);
			}
	}
	
	void OnDrawGizmos()	{
	  Gizmos.DrawIcon(iconPosition, teamName);
	}
}
