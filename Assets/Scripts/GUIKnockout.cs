using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIKnockout : MonoBehaviour {

	public GameObject counter_1;
	public GameObject container_1;
	public GameObject counter_2;
	public GameObject container_2;
	//public GameObject raceManager;

	Text timeLeft;
    
    private int curtime;
    private AudioSource koSfx;

	void Start(){
		container_1.SetActive (false);
		container_2.SetActive (false);
        koSfx = GetComponent<AudioSource>();
    }

	public void startCounter(int number){
		if (number == 1) {
			timeLeft = counter_1.GetComponent<UnityEngine.UI.Text> ();
			container_1.SetActive (true);
		} else if (number == 2) {
			timeLeft = counter_2.GetComponent<UnityEngine.UI.Text> ();
			container_2.SetActive (true);
		} else {
			Debug.Log ("knockout counter out of bound");
		}
        curtime = number;
        koSfx.Play();
	}
	public void tickCounter(int time){
		timeLeft.text = "" + time;
        if (time != curtime)
        {
            koSfx.Play();
            curtime = time;
        }
	}

	public void endCounter(){
		if (container_1.activeSelf == true) {
			container_1.SetActive (false);
		}
		if (container_2.activeSelf == true) {
			container_2.SetActive (false);
		}
	}
}
