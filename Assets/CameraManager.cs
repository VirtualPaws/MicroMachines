using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject camClassic_1;
	public GameObject camClassic_2;
	public GameObject cam3rdPerson_1;
	public GameObject cam3rdPerson_2;

	public GameObject gui_classic;
	public GameObject gui_3rdPerson;

	public GameObject wallColor;
	public GameObject wallPlane;

	private bool isClassic;
	private bool is3rdPerson;
	// Use this for initialization
	void Start () {
		GameObject choiceManager = GameObject.Find("ChoiceManager");
		isClassic = choiceManager.GetComponent<LocalChoiceManager>().isCameraModeClassic ();
		is3rdPerson = choiceManager.GetComponent<LocalChoiceManager>().isCameraMode3rdPerson ();

		camClassic_1.SetActive (isClassic);
		camClassic_2.SetActive (isClassic);
		cam3rdPerson_1.SetActive (is3rdPerson);
		cam3rdPerson_2.SetActive (is3rdPerson);

		wallColor.SetActive (!isClassic);
		wallPlane.SetActive (!isClassic);

		//gui_classic.SetActive (isClassic);
		//gui_3rdPerson.SetActive (is3rdPerson);

	}

}
