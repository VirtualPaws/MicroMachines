using UnityEngine;
using System.Collections;

public class ChooseCam : MonoBehaviour {

	public bool classic;
	public bool shoulder;
	public bool open;
	public GameObject canvasContainer;

	void Start () {
		classic = false;
		shoulder = true;
		open = false;
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
