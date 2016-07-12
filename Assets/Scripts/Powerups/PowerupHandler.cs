using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupHandler : MonoBehaviour {
    public KeyCode powerupKey = KeyCode.RightControl;
    public bool debugFiring = false;

    public bool canPickup = true;
    public bool hasPowerup = false;

    public IPowerUp powerup = null;

    public ParticleSystem speedBoostSystem;

    public bool spawning = false;
    public float spawnInterval = 0.1f; //seconds
    private float lastSpawned= Time.time;
    public GameObject spawningPrefab;
    public float spawnTimeLeft = -1;

    private List<IDelayedBehavior> delayedActions = new List<IDelayedBehavior>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (debugFiring && hasPowerup && Input.GetKeyDown(powerupKey))
        {
            firePowerUp();
        }
        if (spawning && spawningPrefab != null)
        {
            if (Time.time - lastSpawned > spawnInterval)
            {

                Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
                RaycastHit hit;
                Vector3 position;

                if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
                {
                    GameObject spawn = GameObject.Instantiate(spawningPrefab);
                    spawn.transform.position = gameObject.transform.position;
                    lastSpawned = Time.time;

                    spawn.transform.position = hit.point;

                    Quaternion newrot = hit.transform.rotation * Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0));
                    if (this.transform.eulerAngles.x != newrot.eulerAngles.x || this.transform.eulerAngles.z != newrot.eulerAngles.z)
                    {
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(newrot.eulerAngles.x, this.transform.eulerAngles.y, newrot.eulerAngles.z), Time.deltaTime * 10.0f);
                        spawn.transform.rotation = hit.transform.rotation;
                    }
                }
            }
            spawnTimeLeft = spawnTimeLeft - Time.deltaTime;
            if (spawnTimeLeft < 0)
            {
                spawning = false;
            }
        }
        foreach (IDelayedBehavior behavior in delayedActions) {
            behavior.update();
            if (behavior.isFinished())
            {
                delayedActions.Remove(behavior);
            }
        }
	}

    public void triggerPickup()
    {
        GUIMultiplayer gui = GameObject.Find("Canvas").transform.Find(gameObject.name).GetComponent<GUIMultiplayer>();
        switch (Random.Range(0, 3))
        {
            case 0:
                powerup = new RocketPowerUp();
                gui.activatePowerUp("bomb", true);
                break;
            case 1:
                powerup = new SpeedBoostPowerUp();
                gui.activatePowerUp("speedboost", true);
                break;
            case 2:
                powerup = new OilSlickPowerUp();
                gui.activatePowerUp("oil", true);
                break;
        }
        hasPowerup = true;
        canPickup = false;
    }

    public bool isCanPickup()
    {
        return canPickup;
    }

    public void firePowerUp()
    {
        if (powerup != null && hasPowerup)
        {
            powerup.fire(gameObject);
        }
        hasPowerup = false;
        canPickup = true;
        GUIMultiplayer gui = GameObject.Find("Canvas").transform.Find(gameObject.name).GetComponent<GUIMultiplayer>();
        gui.deactivateAll();
    }

    public void addDelayedBehavior(IDelayedBehavior behavior) {
        delayedActions.Add(behavior);
    }
}
