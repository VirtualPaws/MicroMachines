using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar]
    public GameObject playerRepresentation;

	// Use this for initialization
    void Start()
    {
        GameObject nmgo = GameObject.Find("NetworkManager");
        PlayerManagement pm = (PlayerManagement)nmgo.GetComponent(typeof(PlayerManagement));
        foreach (GameObject player in pm.playerRepresentations)
        {
            PlayerRep pr = (PlayerRep)player.GetComponent(typeof(PlayerRep));
            if (pr.hasAuthority)
            {
                //playerRepresentation = player;
                break;
            }
        }
        PlayerRep prep = (PlayerRep)playerRepresentation.GetComponent(typeof(PlayerRep));

        if (prep.isLocalPlayer)
            GetComponent<Driving>().enabled = true;
        GameObject go = GameObject.Find("Main Camera");
        CameraFollowMultiple other = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
        other.addObjectToFollowList(gameObject);
	}

    public void Initialise()
    {
    }
}
