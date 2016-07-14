using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject camClassic_1;
	public GameObject camClassic_2;
	public GameObject cam3rdPerson_1;
	public GameObject cam3rdPerson_2;

	public GameObject wallColor;
	public GameObject wallPlane;

	public bool isClassic;
	public bool is3rdPerson;
	// Use this for initialization
	void Start () {
		GameObject choiceManager = GameObject.Find("ChoiceManager");
		isClassic = choiceManager.GetComponent<LocalChoiceManager>().isCameraModeClassic ();
		is3rdPerson = choiceManager.GetComponent<LocalChoiceManager>().isCameraMode3rdPerson ();

		Debug.Log ("classic" + isClassic + "3rdperson" + is3rdPerson);

		camClassic_1.SetActive (isClassic);
		camClassic_2.SetActive (isClassic);
		cam3rdPerson_1.SetActive (is3rdPerson);
		cam3rdPerson_2.SetActive (is3rdPerson);

		wallColor.SetActive (!isClassic);
		wallPlane.SetActive (!isClassic);


	}

}
