using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerRep : NetworkBehaviour {

    public int id = -1;

    [SyncVar]
    public bool isReady = false;
    public GameObject carPrefab;

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("NetworkManager");
        PlayerManagement pm = (PlayerManagement)go.GetComponent(typeof(PlayerManagement));
        id = pm.playerRepresentations.Count;
        pm.playerRepresentations.Add(gameObject);

        NetworkManager nm = (NetworkManager)gameObject.GetComponent(typeof(NetworkManager));
        if (SceneManager.GetActiveScene().name == "testTrack_2")
        {
            GameObject car = (GameObject)Instantiate(carPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(car);
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
