using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

	//credits to jashan, unityforum
	private static MusicHandler instance = null;
	public static MusicHandler Instance{
		get { return instance; }
	}

	void Awake(){
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (this.gameObject);
	}

	public void playNextSound(){
		
	}
}
