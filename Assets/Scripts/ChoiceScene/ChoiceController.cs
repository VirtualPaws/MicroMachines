using UnityEngine;
using System.Collections;
using System.Linq;

public class ChoiceController : MonoBehaviour {
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
	void Update () {
		if (Input.GetKeyDown("left")) {
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
}
