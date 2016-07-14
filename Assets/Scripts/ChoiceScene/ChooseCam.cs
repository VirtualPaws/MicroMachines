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
    float toggletimer = 0;
    bool canToggle = true;

	void Start () {
		classic = false;
		shoulder = true;
		open = false;
	}

	void Update(){
        if (canToggle)
        {
            if (Input.GetAxis("Powerup1") > 0 || Input.GetAxis("Vertical2") < 0 || Input.GetAxis("Vertical3") < 0)
            {
			    toggleWindow ();
                canToggle = false;
		    }
        }
		if (open) {
            //Bedingungen
            if (Input.GetAxis("Horizontal1") > 0 || Input.GetAxis("Horizontal2") > 0 || Input.GetAxis("Horizontal3") > 0 || Input.GetAxis("Horizontal4") > 0)
            {
				Debug.Log ("grosser 0");
				setShoulder ();
				thrdPersonBtn.Select ();
            }
            else if (Input.GetAxis("Horizontal1") < 0 || Input.GetAxis("Horizontal2") < 0 || Input.GetAxis("Horizontal3") < 0 || Input.GetAxis("Horizontal4") < 0)
            {
				Debug.Log ("kleiner 0");
				setClassic ();
				classicBtn.Select ();
			}

            if (canToggle)
            {
                if (Input.GetAxis("Vertical2") > 0 || Input.GetAxis("Vertical3") > 0)
                {
                    toggleWindow();
                }
            }
		}

        if (!canToggle)
        {
            toggletimer += Time.deltaTime;
            if (toggletimer > 1)
            {
                canToggle = true;
                toggletimer = 0;
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
