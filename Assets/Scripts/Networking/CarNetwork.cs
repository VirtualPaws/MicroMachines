using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CarNetwork : NetworkBehaviour {

    [SyncVar]
    public GameObject owner;
    [SyncVar]
    public bool dirty = false;
    public bool inCamera = false;

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name == "ChoiceScene")
        {
            return;
        }
        Initialise();
        GameObject go = GameObject.Find("Main Camera");
        if (go != null)
        {
            CameraFollowMultiple nm = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
            nm.addObjectToFollowList(gameObject);
            inCamera = true;
        }
	}

    public void Initialise()
    {
        ClientSidePlayer player = (ClientSidePlayer)owner.GetComponent(typeof(ClientSidePlayer));
        if (owner != null && player.isLocalPlayer)
        {
            GetComponent<Driving>().enabled = true;
            if (!hasAuthority)
            {
                Debug.LogError("AUTHORITY PROBLEM");
                player.CmdRespawn(gameObject);
            }
        }
        dirty = false;
        print("Initialised car.");
    }
	
	// Update is called once per frame
	void Update () {
        if (!inCamera)
        {
            GameObject go = GameObject.Find("Main Camera");
            if (go != null)
            {
                CameraFollowMultiple nm = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
                if (nm != null)
                {
                    nm.addObjectToFollowList(gameObject);
                    inCamera = true;
                }
            }
        }
        if (dirty)
        {
            Initialise();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClientSidePlayer player = (ClientSidePlayer)owner.GetComponent(typeof(ClientSidePlayer));
            if (player.isLocalPlayer)
                player.CmdRespawn(gameObject);
        }
	}
}
