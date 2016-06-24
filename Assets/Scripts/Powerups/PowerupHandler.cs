using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupHandler : MonoBehaviour {
    public KeyCode powerupKey = KeyCode.RightControl;

    private bool canPickup = true;
    private bool hasPowerup = false;

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
        if (hasPowerup && Input.GetKeyDown(powerupKey))
        {
            firePowerUp();
        }
        if (spawning && spawningPrefab != null)
        {
            if (Time.time - lastSpawned < spawnInterval)
            {
                GameObject spawn = GameObject.Instantiate(spawningPrefab);
                spawn.transform.position = gameObject.transform.position;
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
        //powerup = new RocketPowerUp();
        powerup = new SpeedBoostPowerUp();
        hasPowerup = true;
        canPickup = false;
    }

    public bool isCanPickup()
    {
        return canPickup;
    }

    public void firePowerUp()
    {
        if (powerup != null)
        {
            powerup.fire(gameObject);
        }
        hasPowerup = false;
        canPickup = true;
    }
    public void addDelayedBehavior(IDelayedBehavior behavior) {
        delayedActions.Add(behavior);
    }
}
