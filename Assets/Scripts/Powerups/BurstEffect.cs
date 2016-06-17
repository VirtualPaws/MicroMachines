using UnityEngine;
using System.Collections;

public class BurstEffect : MonoBehaviour {
    public float lifeTime;
    private float born;
    public GameObject parentObject;

	// Use this for initialization
	void Start () {
        born = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (parentObject != null)
        {
            gameObject.transform.position = parentObject.transform.position;
        }
        if (Time.time > born + lifeTime)
        {
            GetComponent<ParticleSystem>().Stop();
        }
	}
}
