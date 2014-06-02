using UnityEngine;
using System.Collections;

public class CreateServerUI : MonoBehaviour {

	private GameObject mainMenuUI;
	private GameObject createServerUI;
	private UILabel serverNameInput;

	private ServerManager serverManager;

	private void Awake() 
	{
		mainMenuUI = GameObject.Find("MainMenu");
		createServerUI = GameObject.Find("CreateServer");
		serverNameInput = GameObject.Find("ServerNameInputLabel").GetComponent<UILabel>();
		serverManager = GameObject.Find("GlobalScriptObject").GetComponent<ServerManager>();
	}

	public void Back()
	{
		mainMenuUI.SetActive(true);
		createServerUI.SetActive(false);
	}

	public void Create()
	{
		GameData.SERVER_NAME = serverNameInput.text;
		serverManager.Register();
	}
}
