using UnityEngine;
using System.Collections;

public class ServerGameUI : MonoBehaviour {
    
	private UILabel serverNameLabel;

	private void Awake() 
	{
		serverNameLabel = GameObject.Find("ServerNameLabel").GetComponent<UILabel>();
	}

    private void Start() 
	{
		serverNameLabel.text = "Server name: " + GameData.SERVER_NAME;
    }

	public void TerminateServer()
	{
		ServerManager.instance.Unregister();   		
	}
}
