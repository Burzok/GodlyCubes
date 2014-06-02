using UnityEngine;
using System.Collections;

public class TeamSelectUI : MonoBehaviour {
   
    private Networking networking;	

	[SerializeField]
	private GameObject healthBar;
	[SerializeField]
	private GameObject teamSelect;
	[SerializeField]
	private GameObject gameHUD;		

    private void Awake()
	{
        networking = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Networking>();
    }

	private void Start() 
	{
		healthBar.SetActive(false);
		gameHUD.SetActive(false);
	}

	public void SelectRedTeam()
    {    
    	networking.ConnectToGame(Team.TEAM_A);
		healthBar.SetActive(true);
		gameHUD.SetActive(true);
		teamSelect.SetActive(false);
	}

	public void SelectBlueTeam()
	{
		networking.ConnectToGame(Team.TEAM_B);
		healthBar.SetActive(true);
		gameHUD.SetActive(true);
		teamSelect.SetActive(false);
	}

    public void Disconnect()
	{
		Network.Disconnect();
    }
}
