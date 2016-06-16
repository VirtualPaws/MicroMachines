using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public GameObject car1;
	public GameObject car2;
	public GameObject car3;

	// Use this for initialization
	void Start () {
		car1.SetActive (true);
		car2.SetActive (true);
		car3.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			SceneManager.LoadScene ("ChoiceScene");
		}
	}
}
