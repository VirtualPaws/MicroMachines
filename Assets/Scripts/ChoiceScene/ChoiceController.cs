using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

public class ChoiceController : NetworkBehaviour {
	//momentan ausgewählter wagen
	public GameObject car;
	//alle möglichen wagen
	public GameObject[] cars;
	public int counter;

	void Start () {
		//holt sich alle wagen mit dem Car tag und macht sie inaktiv
		cars = GameObject.FindGameObjectsWithTag("Car");
		for (int i = 0; i < cars.Length; i++) {
			cars[i].SetActive (false);
		}

		counter = 0;
		//initialisiert den ersten wagen
		car = (GameObject)Instantiate (cars [counter], cars [counter].transform.position, cars [counter].transform.rotation);
		car.SetActive (true);
	}

	//in der variablen car steckt immer nur das ausgewählte auto
    void Update()
    {
        
		if (Input.GetKeyDown("left")) {
            GameObject player = getLocalPlayerRep();
            if (player == null)
            {
                return;
            }
			Destroy (car);
			counter--;
			if (counter < 0)
				counter = cars.Length-1;
			car = (GameObject)Instantiate (cars[counter], cars[counter].transform.position, cars[counter].transform.rotation);
			car.SetActive (true);
		}
		if (Input.GetKeyDown("right")) {
			Destroy (car);
			counter++;
			if (counter >= cars.Length)
				counter = 0;
			car = (GameObject)Instantiate (cars[counter], cars[counter].transform.position, cars[counter].transform.rotation);
			car.SetActive (true);
		}
	}

    GameObject getLocalPlayerRep()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
        {
            return null;
        }
        foreach (GameObject pl in players)
        {
            ClientSidePlayer clientSidePlayer = (ClientSidePlayer)pl.GetComponent(typeof(ClientSidePlayer));
            if (clientSidePlayer.isLocalPlayer)
            {
                return pl;
            }
        }
        return null;
    }

    PlayerModel getPlayerModel(GameObject player)
    {
        return (PlayerModel)player.GetComponent(typeof(PlayerModel));
    }

    ClientSidePlayer getCLientSidePlayer(GameObject player)
    {
        return (ClientSidePlayer)player.GetComponent(typeof(ClientSidePlayer));
    }
}
