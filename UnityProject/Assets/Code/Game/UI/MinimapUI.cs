using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapUI : MonoBehaviour {

    public Texture2D minimap;
    public Texture2D myPlayerIcon;
    public Texture2D redTeamIcon;
    public Texture2D blueTeamIcon;

    public List<PlayerData> minimapPlayerList;

    private int iconHalfSize;

    private PlayerManager playerManager;

    private int mapOffSetX = Screen.width - 120;
    private int mapOffSetY = Screen.height - 190;

    private int minimapOffsetX = 50;
    private int minimapOffsetY = 165;

    public int miniMapX = 114;
    public int miniMapY = 178;

    private int iconSize = 10;
    private int mapSizeX = 160;
    private int mapSizeY = 250;

	void Awake () {
        playerManager = GameObject.FindWithTag(Tags.gameController).GetComponent<PlayerManager>();
        iconHalfSize = iconSize / 2;
	}

    void OnGUI() {
        DrawMinimap();
    }

    public void AddPlayer(PlayerData p) {
        if(minimapPlayerList.Contains(p) == false)
            minimapPlayerList.Add(p);
    }

    public void RemovePlayer(PlayerData p) {
        if(minimapPlayerList.Contains(p) == true)
            minimapPlayerList.Remove(p);
    }

    private void DrawMinimap() {
        if(GameData.DRAW_MINIMAP == true) { 
            GUI.BeginGroup(new Rect(mapOffSetX, mapOffSetY, miniMapX, miniMapY), minimap);
            foreach (PlayerData player in minimapPlayerList) {
                    if(player.id.isMine)
                        DrawMyPlayer(player);
                    else if(player.team == Team.TEAM_A) {       
                            DrawRedPlayer(player);
                    }
                    else if(player.team == Team.TEAM_B) {
                             DrawBluePlayer(player);
                    }
                }
            GUI.EndGroup();
        }
    }

    private void DrawMyPlayer(PlayerData player) {
        float pX = GetMapPos(player.transform.position.x, miniMapX, mapSizeX) + minimapOffsetX;
        float pZ = GetMapPos(player.transform.position.z, miniMapY, mapSizeY) + minimapOffsetY;
        float playerMapX = pX - iconHalfSize;
        float playerMapZ = ((pZ * -1) - iconHalfSize) + mapSizeY;
        GUI.DrawTexture(new Rect(playerMapX, playerMapZ, iconSize, iconSize), myPlayerIcon);
    }

    private void DrawRedPlayer(PlayerData player) {
        float pX = GetMapPos(player.transform.position.x, miniMapX, mapSizeX) + minimapOffsetX;
        float pZ = GetMapPos(player.transform.position.z, miniMapY, mapSizeY) + minimapOffsetY;
        float playerMapX = pX - iconHalfSize;
        float playerMapZ = ((pZ * -1) - iconHalfSize) + mapSizeY;
        GUI.DrawTexture(new Rect(playerMapX, playerMapZ, iconSize, iconSize), redTeamIcon);
    }

    private void DrawBluePlayer(PlayerData player) {
        float pX = GetMapPos(player.transform.position.x, miniMapX, mapSizeX) + minimapOffsetX;
        float pZ = GetMapPos(player.transform.position.z, miniMapY, mapSizeY) + minimapOffsetY;
        float playerMapX = pX - iconHalfSize;
        float playerMapZ = ((pZ * -1) - iconHalfSize) + mapSizeY;
        GUI.DrawTexture(new Rect(playerMapX, playerMapZ, iconSize, iconSize), blueTeamIcon);
    }

    float GetMapPos(float pos, float mapSize, float sceneSize) {
        return pos * mapSize/sceneSize;
    }
}