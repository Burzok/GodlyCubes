using UnityEngine;
using System.Collections;

public class TowerTeamChoser : MonoBehaviour {
	public Team teamSelect;
	
	private string teamName;
	private Vector3 iconPosition;
	
	void Awake () {
		if (teamSelect != null) {
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
		
		iconPosition = transform.position + new Vector3(0f,3f,0f);
	}
	
	void OnDrawGizmos()	{
	  Gizmos.DrawIcon(iconPosition, teamName);
	}
}
