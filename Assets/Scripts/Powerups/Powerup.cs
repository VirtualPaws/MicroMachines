using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    public static float respawnDelay = 5; //seconds

    private float lastPickedUp = 0;
    private bool uppickable = false;

    public ParticleSystem pickUpParticles;

	// Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (!uppickable && Time.time - lastPickedUp > respawnDelay)
        {
            SetUpPickable(true);
        }
	}

    void OnTriggerEnter(Collider col)
    {
        //Debug.LogError("Collision Occurred");
        PowerupHandler ph = col.gameObject.GetComponent<PowerupHandler>();
        if (ph != null && uppickable && ph.isCanPickup())
        {
            //Debug.LogError("PICKUP");
            lastPickedUp = Time.time;
            SetUpPickable(false);
            ph.triggerPickup();
            ParticleSystem ex = Instantiate(pickUpParticles);
            ex.transform.position = gameObject.transform.position;
            ex.Play();
            AudioSource audio = GetComponent<AudioSource>();
            audio.pitch = Random.Range(0.5f, 1f);
            audio.Play();
            audio.Play(44100);
            return;
        }
        //Debug.LogError("object cannot hold powerups");
    }

    public bool isUpPickable()
    {
        return uppickable;
    }

    private void SetUpPickable(bool value) {
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "SpinnyCube")
            {
                t.gameObject.GetComponent<MeshRenderer>().enabled = value;
                uppickable = value;
                return;
            }
        }
    }
}
