using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIKnockout : MonoBehaviour {

	public GameObject counter;
	public GameObject container;
	//public GameObject raceManager;

	Text timeLeft;
	//private int checkTimeLeft;

	// Use this for initialization
	/*
	void Start () {
		
		container.SetActive (false);
	}*/
	
	// Update is called once per frame
	/*
	void Update () {
		//checkTimeLeft = raceManager.GetComponent<RaceManager> ().getTimeLeft ();

		if (checkTimeLeft > -1) {
			if (!container.activeSelf) {
				container.SetActive (true);
			}
			tickCounter ();
		}
		else if (checkTimeLeft == -1 && container.activeSelf) {
			container.SetActive (false);
		}
	}*/

	public void startCounter(){
		timeLeft =  counter.GetComponent<UnityEngine.UI.Text>();
		container.SetActive (true);

	}
	public void tickCounter(int time){
		timeLeft.text = "" + time;
	}

	public void endCounter(){
		container.SetActive (false);
	}
}
