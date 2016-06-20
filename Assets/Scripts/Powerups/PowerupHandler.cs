using UnityEngine;
using System.Collections;

public class PowerupHandler : MonoBehaviour {
    public KeyCode powerupKey = KeyCode.RightControl;

    private bool canPickup = true;
    private bool hasPowerup = false;

    public IPowerUp powerup = null;

    public ParticleSystem speedBoostSystem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (hasPowerup && Input.GetKeyDown(powerupKey))
        {
            firePowerUp();
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
}
