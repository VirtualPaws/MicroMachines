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

        NetworkManager nm = (NetworkManager)gameObject.GetComponent(typeof(NetworkManager));
        /*
        if (SceneManager.GetActiveScene().name == "testTrack_2")
        {
            GameObject car = (GameObject)Instantiate(carPrefab, transform.position, transform.rotation);
            //SetupLocalPlayer slp = (SetupLocalPlayer)go.GetComponent(typeof(SetupLocalPlayer));
            //slp.CmdSetPlayerId(id);
            //NetworkServer.Spawn(car);
            //slp.CmdSetPlayerId(id);
            //slp.Initialise();
            NetworkServer.SpawnWithClientAuthority(car, gameObject);
        }
         * */

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
