using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerManagement : MonoBehaviour {

    public int playerThreshold = 2;
    public List<GameObject> playerRepresentations;
    private bool inGame = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!inGame && playerRepresentations.Count > 0)
        {
            bool ready = true;
            foreach (GameObject player in playerRepresentations)
            {
                PlayerRep pr = (PlayerRep)player.GetComponent(typeof(PlayerRep));
                ready = ready && pr.isReady;
            }
            if (ready)
            {
                NetworkManager nm = (NetworkManager)gameObject.GetComponent(typeof(NetworkManager));
                nm.ServerChangeScene("testTrack_2");
                playerRepresentations.Clear();
                inGame = true;
            }
        }
	}
}
