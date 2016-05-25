using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerModel : NetworkBehaviour {

    [SyncVar]
    public string carPrefabPath;

    [SyncVar]
    public GameObject car = null;

    [SyncVar]
    public string debugThingie = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdSetCarPrefabPath(string path)
    {
        this.carPrefabPath = path;
    }
}
