using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

    public GameObject toFace;
	
	// Update is called once per frame
	void Update () {
        if (toFace == null)
        {
            return;
        }
        this.transform.LookAt(toFace.transform.position);
        this.transform.Rotate(new Vector3(0, 180, 0));
	}
}
