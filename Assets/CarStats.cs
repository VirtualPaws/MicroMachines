using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarStats : MonoBehaviour {
	
	public GameObject choiceManager;
	public GameObject speed;
	public GameObject turn;
	public GameObject bump;
	public int index;

	public List<GameObject> picks;
	// Use this for initialization
	void Start () {
		speed.SetActive (false);
		turn.SetActive (false);
		bump.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		picks = choiceManager.GetComponent<LocalChoiceManager> ().getPicks ();
		switch(picks[index].name.ToString()){
		case "Car_Firefighter":
			speed.SetActive (false);
			turn.SetActive (true);
			bump.SetActive (false);
			break;
		case "Car_IceCream":
			speed.SetActive (false);
			turn.SetActive (false);
			bump.SetActive (true);
			break;
		case "Car_Taxi":
			speed.SetActive (true);
			turn.SetActive (false);
			bump.SetActive (false);
			Debug.Log ("taxi");
			break;
		}
	}
		
}
