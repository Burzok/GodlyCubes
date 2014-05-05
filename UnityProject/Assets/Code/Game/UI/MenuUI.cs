using UnityEngine;
using System.Collections.Generic;

public class MenuUI : SingletonComponent<MenuUI> {
		
    private CreateServerUI createServerUI;
    private ClientConnectUI clientConnectUI;
    private OptionsUI optionsUI;
	
	void Start() {
		GameData.instance.gameDataAsset.CURRENT_LEVEL_NAME = "DeathMatch";
		GameData.instance.gameDataAsset.DRAW_CHAT = false;
		GameData.instance.gameDataAsset.DRAW_STATS = false;
		
		SetMainMenuState();
		
        createServerUI = GetComponent<CreateServerUI>();
        clientConnectUI = GetComponent<ClientConnectUI>();
        optionsUI = GetComponent<OptionsUI>();
	}
	
	private void DrawMainMenu() {
		if (GUI.Button (new Rect(Screen.width * 0.5f-120f, 20f, 100f, 50f), "Create Server"))
			createServerUI.SetCreateServerState();

		if (GUI.Button (new Rect(Screen.width * 0.5f+20f, 20f, 100f, 50f), "Connect")) 
			clientConnectUI.SetConnectState();
		
		if (GUI.Button(new Rect(Screen.width * 0.5f-50f, Screen.height * 0.5f, 100f, 50f), "Options")) 
			optionsUI.SetOptionsMenuState();

		if (GUI.Button (new Rect(Screen.width * 0.5f-50f, Screen.height - 70f, 100f, 50f),"Quit"))
			Application.Quit();
	}
	
	private void Dumy()
	{}
				
	public void SetMainMenuState() {
        DrawUI.instance.drawUI = DrawMainMenu;
	}
}
