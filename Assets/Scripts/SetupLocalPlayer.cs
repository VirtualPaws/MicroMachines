using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
            GetComponent<Driving>().enabled = true;
        GameObject go = GameObject.Find("Main Camera");
        CameraFollowMultiple other = (CameraFollowMultiple)go.GetComponent(typeof(CameraFollowMultiple));
        GameObject[] newArray = new GameObject[other.objectsToFollow.Length + 1];
        for (int i = 0; i < other.objectsToFollow.Length; i++)
        {
            newArray[i+1] = other.objectsToFollow[i];
        }
        newArray[0] = gameObject;
        other.objectsToFollow = newArray;
	}
}
