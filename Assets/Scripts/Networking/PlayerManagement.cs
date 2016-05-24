using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerManagement : MonoBehaviour {

    public int playerThreshold = 2;
    public List<GameObject> playerRepresentations;
    public List<GameObject> cars;
    private bool inGame = false;
    private bool carsSpawned = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!inGame && playerRepresentations.Count >= playerThreshold)
        {
            bool ready = true;
            foreach (GameObject player in playerRepresentations)
            {
                if (player == null)
                {
                    continue;
                }
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
        if (inGame && !carsSpawned)
        {
            NetworkManager nm = (NetworkManager)gameObject.GetComponent(typeof(NetworkManager));
            foreach (GameObject player in playerRepresentations)
            {
                if (player == null)
                {
                    continue;
                }
                PlayerRep pRep = (PlayerRep)player.GetComponent(typeof(PlayerRep));
                GameObject car = (GameObject)Instantiate(pRep.carPrefab, transform.position, transform.rotation);
                SetupLocalPlayer slp = (SetupLocalPlayer)car.GetComponent(typeof(SetupLocalPlayer));
                slp.playerRepresentation = player;
                NetworkServer.SpawnWithClientAuthority(car, player);
                cars.Add(car);
                carsSpawned = true;
            }
        }
	}
}
