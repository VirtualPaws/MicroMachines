using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerSidePlayermanagement : NetworkBehaviour {

    public List<GameObject> players;
    bool inGame = false;
    bool carsSpawned = false;
    int spawnPointNum = -1;

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
                foreach (GameObject player in players)
                {
                    ((ClientSidePlayer)player.GetComponent(typeof(ClientSidePlayer))).isReady = false;
                }
                GameObject go = GameObject.Find("NewNetworkManager");
                NetworkManager nm = (NetworkManager)go.GetComponent(typeof(NetworkManager));
                NetworkServer.SetAllClientsNotReady();
                nm.ServerChangeScene("textureScene");
                inGame = true;
            }
        }
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        if (inGame && cars.Length < players.Count)
        {
            bool ready = true;
            foreach (GameObject player in players)
            {
                ready = ready && ((ClientSidePlayer)player.GetComponent(typeof(ClientSidePlayer))).isReady;
            }
            if (ready)
            {
                foreach (GameObject player in players)
                {
                    spawnCar(player);
                }
                carsSpawned = true;
            }
        }
        if (inGame && carsSpawned && cars.Length >= players.Count)
        {
            foreach (GameObject player in players)
            {
                PlayerModel playerModel = (PlayerModel)player.GetComponent(typeof(PlayerModel));
                if (playerModel.car == null)
                {
                    print("Detected player model without car.");
                    playerModel.debugThingie = playerModel.debugThingie + "Detected player model without car. ";
                    GameObject[] carss = GameObject.FindGameObjectsWithTag("Car");
                    bool success = false;
                    foreach (GameObject car in carss)
                    {
                        CarNetwork carNetWork = (CarNetwork)car.GetComponent(typeof(CarNetwork));
                        if (carNetWork.owner.GetInstanceID() == player.GetInstanceID())
                        {
                            playerModel.car = car;
                            success = true;
                            carNetWork.dirty = true;
                            break;
                        }
                    }
                    if (success)
                    {
                        print("Successfully set car to player model.");
                        playerModel.debugThingie = playerModel.debugThingie + "Successfully set car to player model. ";
                    }
                    else
                    {
                        print("Failed to set car to player model.");
                        playerModel.debugThingie = playerModel.debugThingie + "Failed to set car to player model. ";
                    }
                }
            }
        }
    }

    [Server]
    public void spawnCarAtSpawnPoint(GameObject owner, GameObject spawnPoint)
    {
        PlayerModel playerModel = (PlayerModel)owner.GetComponent(typeof(PlayerModel));
        GameObject carPrefab = (GameObject)Resources.Load(playerModel.carPrefabPath);
        GameObject car = (GameObject)Instantiate(carPrefab, transform.position, transform.rotation);
        CarNetwork carNetwork = (CarNetwork)car.GetComponent(typeof(CarNetwork));
        carNetwork.owner = owner;
        car.transform.position = spawnPoint.transform.position;
        car.transform.rotation = spawnPoint.transform.rotation;
        car.transform.Translate(new Vector3(0, 15, 0));
        NetworkServer.SpawnWithClientAuthority(car, owner);
        playerModel.car = car;
    }

    [Server]
    public void spawnCar(GameObject owner)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpunkt");
        spawnPointNum = (spawnPointNum + 1) % spawnPoints.Length;
        spawnCarAtSpawnPoint(owner, spawnPoints[spawnPointNum]);
    }

    /*
    [Server]
    public void killCar(GameObject owner)
    {
        GameObject[] carss = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in carss)
        {
            CarNetwork carNetWork = (CarNetwork)car.GetComponent(typeof(CarNetwork));
            Debug.LogError(car);
            try
            {
                if (carNetWork.owner == owner)
                {
                    Destroy(car);
                    Debug.LogError("DESTROYED");
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(carNetWork);
                foreach (Component comp in car.GetComponents(typeof(Component)))
                {
                    Debug.LogError(comp.name);
                }
                //Debug.LogError(carNetWork.owner);
            }
        }
    }
     * */

    [Server]
    public void killCar(GameObject car)
    {
        NetworkServer.Destroy(car);
    }

    [Server]
    public void RegisterPlayer(GameObject player)
    {
        players.Add(player);
        PlayerModel playerModel = (PlayerModel)player.GetComponent(typeof(PlayerModel));
    }
}
