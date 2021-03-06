﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapRangeDetector : MonoBehaviour {

    private PlayerData checkingPlayer;
    private PlayerData collidingPlayerData;
    private RaycastHit hitInfo;

    void Awake() {
        checkingPlayer = transform.parent.GetComponent<PlayerData>();
    }

    void Update() {
        if (PlayerManager.instance.myPlayer != null) 
            AddPlayerToMinimapList();
    }

    private void AddPlayerToMinimapList()  {
        if (PlayerManager.instance.myPlayer.team == checkingPlayer.team)
            MinimapPlayerList.instance.AddPlayer(checkingPlayer);
    }

    void OnTriggerStay(Collider collidingPlayer) {
        collidingPlayerData = collidingPlayer.transform.parent.GetComponent<PlayerData>();
        if(collidingPlayerData.team != checkingPlayer.team)
            CheckPlayerVisible(collidingPlayerData);
    }

    void OnTriggerExit(Collider collidingPlayer){
        if (collidingPlayer.transform.parent.GetComponent<PlayerData>().team != PlayerManager.instance.myPlayer.team &&
            collidingPlayer.transform.parent.GetComponent<PlayerData>().team != checkingPlayer.team)
                MinimapPlayerList.instance.RemovePlayer(collidingPlayerData);
    }

    public void CheckPlayerVisible(PlayerData collidingPlayerData) {
        Vector3 direction = collidingPlayerData.transform.position - checkingPlayer.transform.position;
        direction = direction.normalized;

        Physics.Raycast(checkingPlayer.transform.position, direction, out hitInfo);
        if (hitInfo.collider == collidingPlayerData.collider)
            MinimapPlayerList.instance.AddPlayer(collidingPlayerData);
        else {
            if (collidingPlayerData.team != PlayerManager.instance.myPlayer.team &&
                collidingPlayerData.team != checkingPlayer.team)
                    MinimapPlayerList.instance.RemovePlayer(collidingPlayerData);
        }
    }
}