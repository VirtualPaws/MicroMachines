using UnityEngine;
using System.Collections;

public class PickupAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float rot = Time.deltaTime * 100;
        transform.Rotate(new Vector3(1, 1, 0), rot);
	}
}
