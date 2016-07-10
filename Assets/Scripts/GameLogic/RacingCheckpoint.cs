using UnityEngine;
using System.Collections;

public class RacingCheckpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Car"))
        {
            GameObject raMan = GameObject.Find("RaceManager");
            if (raMan == null)
            {
                Debug.LogError("No Racemanager found!");
                return;
            }
            raMan.GetComponent<RaceManager>().hitCheckpoint(this, other.gameObject);
        }
    }
}
