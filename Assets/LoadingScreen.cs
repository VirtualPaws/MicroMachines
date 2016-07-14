using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Example ());
	
	}

	IEnumerator Example(){
		yield return new WaitForSeconds (8);
		SceneManager.LoadScene("textureScene");
	}
}
