using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class ServerSidePlayermanagement : NetworkBehaviour {

    public List<GameObject> players;
    bool inGame = false;
    bool carsSpawned = false;

	// Use this for initialization
    [Server]
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
    [Server]
	void Update () {
        if (players.Count > 0 && !inGame)
        {
            bool ready = true;
            foreach (GameObject player in players)
            {
                ready = ready && ((ClientSidePlayer)player.GetComponent(typeof(ClientSidePlayer))).isReady;
            }
            if (ready)
            {
                GameObject go = GameObject.Find("NewNetworkManager");
                NetworkManager nm = (NetworkManager)go.GetComponent(typeof(NetworkManager));
                nm.ServerChangeScene("textureScene");
                inGame = true;
            }
        }
		GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        if (inGame && !carsSpawned && cars.Length < players.Count)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpunkt");
            int num = 1;
            foreach (GameObject player in players)
            {
                spawnCar(player, spawnPoints[num]);
                num++;
            }
        }
	}

    [Server]
    public void spawnCar(GameObject owner, GameObject spawnPoint)
    {
        PlayerModel playerModel = (PlayerModel)owner.GetComponent(typeof(PlayerModel));
        GameObject carPrefab = (GameObject)Resources.Load(playerModel.carPrefabPath);
        GameObject car = (GameObject)Instantiate(carPrefab, transform.position, transform.rotation);
        CarNetwork carNetwork = (CarNetwork)car.GetComponent(typeof(CarNetwork));
        carNetwork.owner = owner;
        car.transform.position = spawnPoint.transform.position;
        car.transform.Translate(new Vector3(0,15,0));
        NetworkServer.SpawnWithClientAuthority(car, owner);
        playerModel.car = car;
    }

    [Server]
    public void RegisterPlayer(GameObject player)
    {
        players.Add(player);
    }
}
