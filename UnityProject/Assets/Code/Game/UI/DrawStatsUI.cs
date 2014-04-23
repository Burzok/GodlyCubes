using UnityEngine;
using System.Collections;

public class DrawStatsUI : MonoBehaviour {
   
    public void DrawStats() {
		if (GameData.instance.gameDataAsset.DRAW_STATS == true)
        {
            GUI.Box(new Rect(5f, 50f, 140f, 50f), "");
            GUI.BeginGroup(new Rect(5f, 50f, 140f, 50f));
            foreach (PlayerData player in ServerManager.instance.playerList)
            {
                GUILayout.BeginHorizontal();

                if (player.color == new Vector3(1, 0, 0))
                    GUI.contentColor = Color.red;
                if (player.color == new Vector3(0, 0, 1))
                    GUI.contentColor = Color.blue;

                GUILayout.Space(5f);
                GUILayout.Label(player.playerName + ": ");

                GUI.contentColor = Color.green;
                GUILayout.Label("K: " + player.kills);
                GUILayout.FlexibleSpace();

                GUI.contentColor = Color.red;
                GUILayout.Label("D: " + player.deaths);
                GUILayout.FlexibleSpace();

                GUI.contentColor = Color.yellow;
                GUILayout.Label("A: " + player.assist);
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();
            }
            GUI.EndGroup();
        }
    }

}
