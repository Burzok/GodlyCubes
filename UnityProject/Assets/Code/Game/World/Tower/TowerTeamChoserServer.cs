using UnityEngine;
using System.Collections;

public class TowerTeamChoserServer : MonoBehaviour {
	public Team teamSelect;
	
	private string teamName;
	private Vector3 iconPosition;
	
	void Awake () {
		teamName = "TowerNeutral";
		iconPosition = transform.position + new Vector3(0f,5f,0f);
	}
	
	public void SetTeam(ref Team team) {
		teamSelect = team;
		if (teamSelect == Team.TEAM_A) {
			teamName = "TowerTeamA";
			gameObject.layer = 13;
			transform.GetChild(0).gameObject.layer = 9;
			GetComponent<TowerReloaderServer>().SetTeam(Team.TEAM_A);
		}
		else if (teamSelect == Team.TEAM_B) {
			teamName = "TowerTeamB";
			gameObject.layer = 14;
			transform.GetChild(0).gameObject.layer = 10;
			GetComponent<TowerReloaderServer>().SetTeam(Team.TEAM_B);
		}
	}
	
	void OnDrawGizmos()	{
	  Gizmos.DrawIcon(iconPosition, teamName);
	}
}
