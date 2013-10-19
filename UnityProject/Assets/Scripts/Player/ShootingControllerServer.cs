using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingControllerServer : MonoBehaviour {
    public Transform bulletPrefabServer;

    private PlayerData data;

    private Transform bullet;
    private Transform bulletSpawner;
    private NetworkViewID bulletID;

    void Awake() {
        networkView.group = 0;
        bulletSpawner = this.transform.FindChild("BulletSpawner");
        data = GetComponent<PlayerData>();
    }

    [RPC]
    void ServerSpawnBulletCall(NetworkViewID owner) {
        bullet = Instantiate(
            bulletPrefabServer, bulletSpawner.transform.position, bulletSpawner.rotation) as Transform;
        bullet.GetComponent<BulletControllerServer>().owner = owner;

        bulletID = AllocateBulletID();
        bullet.networkView.viewID = bulletID;
        networkView.RPC("SpawnBulletOnClient", RPCMode.Others, owner, bulletID);

        SetLayers();
        SetColor();
    }

    [RPC]
    void SpawnBulletOnClient(NetworkViewID owner, NetworkViewID bulletID) {}

    private NetworkViewID AllocateBulletID() {
        return Network.AllocateViewID();
    }

    private void SetLayers() {
        if (data.team == Team.TEAM_A)
            bullet.gameObject.layer = 11;
        else if (data.team == Team.TEAM_B)
            bullet.gameObject.layer = 12;
    }

    private void SetColor() {
        bullet.GetChild(0).renderer.material.color = new Color(data.color.x, data.color.y, data.color.z);
    }
}
