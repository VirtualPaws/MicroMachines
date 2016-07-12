using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMultiplayer : MonoBehaviour {

	public GameObject bomb;
	public GameObject flask;
	public GameObject lightning;

	public GameObject textfield1;
	public GameObject textfield2;
	public UnityEngine.UI.Text score;
	public UnityEngine.UI.Text scoreShadow;

	private int playerScore;

	// Use this for initialization
	void Start () {
		//score textzeug
		score = textfield2.GetComponent<UnityEngine.UI.Text> ();
		scoreShadow = textfield1.GetComponent<UnityEngine.UI.Text> ();
		playerScore = 0;
		refreshScore ();

		//power up zeug
		bomb.SetActive (false);
		flask.SetActive (false);
		lightning.SetActive (false);
	}
		
	public void refreshScore(){
		score.text = "" + playerScore;
		scoreShadow.text = "" + playerScore;
        Canvas.ForceUpdateCanvases();
        textfield1.SetActive(false);
        textfield2.SetActive(false);
        textfield1.SetActive(true);
        textfield2.SetActive(true);
	}

	public void setScore(int number){
		playerScore = number;
		refreshScore ();
	}

	public void addScore(int number){
		playerScore += number;
		refreshScore ();
	}

	public void resetScore(int number){
		playerScore = 0;
		refreshScore ();
	}

	public void activatePowerUp(string name, bool active){
		switch (name) {
			case "bomb":
				bomb.SetActive (active);
				lightning.SetActive (!active);
				flask.SetActive (!active);
				break;
			case "speedboost":
				lightning.SetActive (active);
				bomb.SetActive (!active);
				flask.SetActive (!active);
				break;
			case "oil":
				flask.SetActive (active);
				lightning.SetActive (!active);
				bomb.SetActive (!active);
				break;
		}
	}

	public void deactivateAll(){
		bomb.SetActive (false);
		flask.SetActive (false);
		lightning.SetActive (false);
	}
				
}
