using UnityEngine;
using System.Collections;

public class ClientGameUI : MonoBehaviour {

    private DrawStatsUI drawStatsUI;

	[SerializeField]
	private UIWidget chatWindow;
	[SerializeField]
	private GameObject chatInput;
	[SerializeField]
	private GameObject disconnectButton;

	private bool hudShow;
	
	private void Start()
	{
		chatWindow.alpha = 0.1f;
		chatInput.SetActive(false);
		disconnectButton.SetActive(false);
		hudShow = false;

	}

	private void Update() 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!hudShow)
			{			
				chatWindow.alpha = 0.9f;
				chatInput.SetActive(true);
				disconnectButton.SetActive(true);
				hudShow = true;
			}
			else
			{
				chatWindow.alpha = 0.1f;
				chatInput.SetActive(false);
				disconnectButton.SetActive(false);
				hudShow = false;
			}
		}
	}

    public void Disconnect()
	{
		Network.Disconnect();			
		Application.LoadLevel(GameData.LEVEL_MAIN_MENU);
		Destroy(gameObject);
    }
}
