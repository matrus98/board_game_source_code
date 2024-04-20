using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SerializeField]
    GameObject Test;

    public Material PlayerMaterial { get; private set; }
    private GameObject MyObject;

    void Start () {
        if (!isLocalPlayer)
            return;

        //PlayerMaterial = FindObjectOfType<GlobalSettingsPlayerColor>().GetRandomPlayerPawnColor();

        //Obiekt do celowo Spawnowany
        //MyObject = FindObjectOfType<GameManager>().MatchMyMaterialToMyPawnColor(PlayerMaterial);

        CmdSpawnMyPersonalPawn();
	}

    void Update () {
        if (!hasAuthority)
            return;
	}

    [Command]
    private void CmdSpawnMyPersonalPawn()
    {
        GameObject go = Instantiate(Test);
        go.transform.position = new Vector3(4.5f, 0.01f, 4.5f);
        //FindObjectOfType<GameManager>().AddPlayerPawn(go);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
