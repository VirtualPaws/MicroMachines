using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerRep : NetworkBehaviour {

    [SyncVar]
    public int id = -1;

    [SyncVar]
    public bool isReady = false;

    [SyncVar]
    public GameObject carPrefab;

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("NetworkManager");
        PlayerManagement pm = (PlayerManagement)go.GetComponent(typeof(PlayerManagement));
        id = pm.playerRepresentations.Count;
        pm.playerRepresentations.Add(gameObject);

	}
	
	// Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if (this.isLocalPlayer && SceneManager.GetActiveScene().name != "testTrack_2")
        {
            CmdSetReady(true);
        }
	}

    [Command]
    public void CmdSetReady(bool ready)
    {
        this.isReady = ready;
    }
}
