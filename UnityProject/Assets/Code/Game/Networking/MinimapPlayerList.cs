using UnityEngine;
using System.Collections.Generic;

public class MinimapPlayerList : SingletonComponent<MinimapPlayerList> {

    public List<PlayerData> minimapPlayerList = new List<PlayerData>();

    public void AddPlayer(PlayerData p) {
        if (minimapPlayerList.Contains(p) == false)
            minimapPlayerList.Add(p);
    }

    public void RemovePlayer(PlayerData p) {
        if (minimapPlayerList.Contains(p) == true)
            minimapPlayerList.Remove(p);
    }
}