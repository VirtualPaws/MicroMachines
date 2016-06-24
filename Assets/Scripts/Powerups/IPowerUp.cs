using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public interface IPowerUp {
    void fire(GameObject firingFrom);
    string getName();
	void endEffect();
}

public class SpeedBoostPowerUp : IPowerUp
{
	public MotionBlur blur = Camera.main.GetComponent<MotionBlur>(); 
    private const float force = 200;
    private const float blurTime = 2; //seconds

    public void fire(GameObject firingFrom)
    {
		//activate motion blur
		blur.blurAmount = 0.9f;
        DelayedBlurReset blurReset = new DelayedBlurReset();
        blurReset.setTimer(blurTime * 0.3f);
        blurReset.setBlurValue(0.7f);
        firingFrom.GetComponent<PowerupHandler>().addDelayedBehavior(blurReset);
        blurReset = new DelayedBlurReset();
        blurReset.setTimer(blurTime * 0.6f);
        blurReset.setBlurValue(0.5f);
        firingFrom.GetComponent<PowerupHandler>().addDelayedBehavior(blurReset);
        blurReset = new DelayedBlurReset();
        blurReset.setTimer(blurTime * 1);
        blurReset.setBlurValue(0.3f);
        firingFrom.GetComponent<PowerupHandler>().addDelayedBehavior(blurReset);

        firingFrom.GetComponent<Rigidbody>().AddRelativeForce(0, 0, force, ForceMode.Impulse);
        ParticleSystem ex = GameObject.Instantiate(firingFrom.GetComponent<PowerupHandler>().speedBoostSystem);
        ex.transform.position = firingFrom.transform.position;
        ex.transform.rotation = firingFrom.transform.rotation;
        ex.transform.Rotate(new Vector3(0, 1, 0), 180);

        ex.Play();
	

    }

    public string getName()
    {
        return "Speed Boost";
    }

	public void endEffect()
	{
		Debug.Log ("end effect");
		blur.blurAmount = 0.3f;
	}


}

public class RocketPowerUp : IPowerUp
{
    private GameObject rocketPrefab = null;
    private Vector3 offset = new Vector3(0, 5, 0);

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

    public void endEffect()
    {
        Debug.Log("end effect");
    }
}

public class OilSlickPowerup : IPowerUp
{
    private GameObject oilPrefab = null;
    private Vector3 offset = new Vector3(0, 5, 0);

    public void fire(GameObject firingFrom)
    {
        if (oilPrefab == null)
        {
            oilPrefab = Resources.Load("attacks/Oil_Attack", typeof(GameObject)) as GameObject;
        }
    }

    public string getName()
    {
        return "Rocket";
    }

    public void endEffect()
    {
        Debug.Log("end effect");
    }
}
