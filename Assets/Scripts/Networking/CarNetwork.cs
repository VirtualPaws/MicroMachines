using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CarNetwork : NetworkBehaviour {

    [SyncVar]
    public GameObject owner;
    public bool inCamera = false;

	// Use this for initialization
	void Start () {
        ClientSidePlayer player = (ClientSidePlayer)owner.GetComponent(typeof(ClientSidePlayer));
        if (owner != null && player.isLocalPlayer)
        {
            GetComponent<Driving>().enabled = true;
        }
        GameObject go = GameObject.Find("Main Camera");
        if (go != null)
        {
            CameraFollowMultiple nm = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
            nm.addObjectToFollowList(gameObject);
            inCamera = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!inCamera)
        {
            GameObject go = GameObject.Find("Main Camera");
            if (go != null)
            {
                CameraFollowMultiple nm = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
                nm.addObjectToFollowList(gameObject);
                inCamera = true;
            }
        }
	}
}
