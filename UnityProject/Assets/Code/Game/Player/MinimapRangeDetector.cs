using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapRangeDetector : MonoBehaviour {

    private PlayerData checkingPlayer;
    private PlayerData collidingPlayerData;
    private RaycastHit hitInfo;

    private MinimapUI minimapUI;
    private bool checkingPlayerAdded = false;

    void Awake() {
        checkingPlayer = transform.parent.GetComponent<PlayerData>();
        minimapUI = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<MinimapUI>();
    }

    void Update() {
        if (PlayerManager.instance.myPlayer != null && !checkingPlayerAdded) { 
            AddPlayerToMinimapList();
            checkingPlayerAdded = true;
        }
    }

    void OnTriggerStay(Collider collidingPlayer) {
        collidingPlayerData = collidingPlayer.transform.parent.GetComponent<PlayerData>();
        if(collidingPlayerData.team != checkingPlayer.team)
            CheckPlayerVisible(collidingPlayerData);
    }

    void OnTriggerExit(Collider collidingPlayer){
        if (collidingPlayer.transform.parent.GetComponent<PlayerData>().team != PlayerManager.instance.myPlayer.team)
            minimapUI.RemovePlayer(collidingPlayerData);
        collidingPlayerData = null;
    }

    public void CheckPlayerVisible(PlayerData collidingPlayer) {
        Vector3 direction = collidingPlayer.transform.position - checkingPlayer.transform.position;
        direction = direction.normalized;

        Physics.Raycast(checkingPlayer.transform.position, direction, out hitInfo);
        if (hitInfo.collider == collidingPlayer.collider)
            minimapUI.AddPlayer(collidingPlayer);
        else
            return;
    }

    private void AddPlayerToMinimapList() {
        if (PlayerManager.instance.myPlayer.team == checkingPlayer.team)
            minimapUI.AddPlayer(checkingPlayer);
    }
}