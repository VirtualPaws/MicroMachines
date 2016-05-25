using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientSidePlayer : NetworkBehaviour {

    [SyncVar]
    public bool isReady = false;

	// Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (hasAuthority)
        {
            this.CmdRegister();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdSetReady(bool ready)
    {
        this.isReady = ready;
    }

    [Command]
    public void CmdRegister()
    {
        GameObject go = GameObject.Find("ServStuff");
        ServerSidePlayermanagement pm = (ServerSidePlayermanagement)go.GetComponent(typeof(ServerSidePlayermanagement));
        pm.RegisterPlayer(gameObject);
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject go = GameObject.Find("ServStuff");
        ServerSidePlayermanagement pm = (ServerSidePlayermanagement)go.GetComponent(typeof(ServerSidePlayermanagement));
        pm.spawnCar(gameObject);
    }

    [Command]
    public void CmdRespawn()
    {
        GameObject go = GameObject.Find("ServStuff");
        ServerSidePlayermanagement pm = (ServerSidePlayermanagement)go.GetComponent(typeof(ServerSidePlayermanagement));
        pm.killCar(gameObject);
        pm.spawnCar(gameObject);
    }

    public void OnLoad()
    {
        if (hasAuthority)
        {
            CmdSetReady(true);
        }
    }
}
