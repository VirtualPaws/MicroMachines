using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
	public GameObject countdownField;
	public GameObject countdownFieldShadow;

	public float delay = 0.5f;
	string message;
	Text textcomp;
	Text textcompShadow;

	// Use this for initialization
	void Start () {
		//countdown zeug
		textcomp = countdownField.GetComponent<UnityEngine.UI.Text>();
		textcomp.text = "";
		textcompShadow = countdownFieldShadow.GetComponent<UnityEngine.UI.Text>();
		textcompShadow.text = "";

		message = "321! ";
		StartCoroutine (TypeText () );
	}

	IEnumerator TypeText(){
		foreach (char letter in message.ToCharArray()) {
			textcomp.text = "" + letter;
			textcompShadow.text = "" + letter;

			yield return new WaitForSeconds (delay);
		}
		countdownField.SetActive (false);
		countdownFieldShadow.SetActive (false);
	}
		
}
