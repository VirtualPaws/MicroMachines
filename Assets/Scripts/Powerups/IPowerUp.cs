using UnityEngine;
using System.Collections;

public interface IPowerUp {
    void fire(GameObject firingFrom);
    string getName();
}

public class SpeedBoostPowerUp : IPowerUp
{
    private const float force = 200;

    public void fire(GameObject firingFrom)
    {
        firingFrom.GetComponent<Rigidbody>().AddRelativeForce(0, 0, force, ForceMode.Impulse);
        ParticleSystem ex = GameObject.Instantiate(firingFrom.GetComponent<PowerupHandler>().speedBoostSystem);
        ex.transform.position = firingFrom.transform.position;
        ex.transform.Rotate(new Vector3(0, 1, 0), 90);
        ex.Play();
    }

    public string getName()
    {
        return "Speed Boost";
    }
}

public class RocketPowerUp : IPowerUp
{
    private GameObject rocketPrefab = null;
    private Vector3 offset = new Vector3(0,5,0);

    public void fire(GameObject firingFrom)
    {
        Vector3 origin = firingFrom.transform.position;
        Vector3 originForce = firingFrom.GetComponent<Rigidbody>().velocity;
        if (rocketPrefab == null)
        {
            rocketPrefab = Resources.Load("attacks/Rocket", typeof(GameObject)) as GameObject;
        }
        GameObject rocket = GameObject.Instantiate(rocketPrefab);
        rocket.transform.position = origin + offset;
        rocket.GetComponent<Rigidbody>().velocity = originForce;
        rocket.GetComponent<AttackRocket>().toIgnore.Add(firingFrom);
    }

    public string getName()
    {
        return "Rocket";
    }
}
