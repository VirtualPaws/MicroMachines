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
    }

    public string getName()
    {
        return "Speed Boost";
    }
}
