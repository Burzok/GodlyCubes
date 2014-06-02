using UnityEngine;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour {
		
	private GameObject mainMenuUI;
	private GameObject createServerUI;
	private GameObject clientConnectUI;

	private void Awake()
	{
		mainMenuUI = gameObject.transform.FindChild("MainMenu").gameObject;
		createServerUI = gameObject.transform.FindChild("CreateServer").gameObject;
		clientConnectUI = gameObject.transform.FindChild("ConnectServer").gameObject;
	}

	private void Start()
	{
		createServerUI.SetActive(false);
		clientConnectUI.SetActive(false);

	}

	public void CreateServer()
	{
		createServerUI.SetActive(true);
		mainMenuUI.SetActive(false);
	}

	public void ConnectServer()
	{
		clientConnectUI.SetActive(true);
		mainMenuUI.SetActive(false);
	}
	
	public void QuitGame()	{
		Application.Quit();
	}
}
