using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseCam : MonoBehaviour {

	public bool classic;
	public bool shoulder;
	public bool open;
	public GameObject canvasContainer;
	public Button classicBtn;
	public Button thrdPersonBtn;

	void Start () {
		classic = false;
		shoulder = true;
		open = false;
	}

	void Update(){
		if (Input.GetButtonDown ("Fire3")) {
			toggleWindow ();
		}
		if (open) {
			if (Input.GetAxis ("Horizontal1") > 0) {
				Debug.Log ("grosser 0");
				setShoulder ();
				thrdPersonBtn.Select ();
			} else if (Input.GetAxis ("Horizontal1") < 0) {
				Debug.Log ("kleiner 0");
				setClassic ();
				classicBtn.Select ();
			}
			if (Input.GetButtonDown ("Fire2")) {
				Debug.Log ("auswählen geht noch nicht");
				canvasContainer.SetActive (false);
			}
		}
	}
	public void toggleWindow(){
		open = !open;
		canvasContainer.SetActive(open);
	}

	public void setClassic(){
		classic = true;
		shoulder = false;
	}	

	public void setShoulder(){
		classic = false;
		shoulder = true;
	}	

	public bool isClassic(){
		return classic;
	}

	public bool isShoulder(){
		return shoulder;
	}
}
