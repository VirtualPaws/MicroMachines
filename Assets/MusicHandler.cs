using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {
	public AudioSource audio1;
	public AudioSource audio2;
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
			audio1 = GetComponent<AudioSource>();
			audio2 = GetComponent<AudioSource>();
			if (Application.loadedLevelName == "EndScene") {
				audio1.Stop ();
				audio2.Play ();
			} else {
				audio1.Play ();
			}
		}
		DontDestroyOnLoad (this.gameObject);
	}

}
