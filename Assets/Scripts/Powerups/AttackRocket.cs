using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRocket : MonoBehaviour
{
    private float timeFired = 0;

    public List<GameObject> toIgnore;
    public float maxSpeed = 1;
    public float timeIdle = 1f; //seconds
    public GameObject target;
    public float forwardModifier = 0.0f;
    public float rotationSpeed = 10;
    public float rotationBasedSpeedMod = 0.9f;
    public float brake = 0.5f;
    public float explosionDistance = 20;
    public float explosionForce = 100;
    public float explosionRadius = 100;
    public float maxFlightTime = 5; //seconds
    public bool debug_neverExplode = false;
    public Vector3 explosionCorrection;
    public ParticleSystem explosion;
    public ParticleSystem blast;
    public ParticleSystem spawn;

    public Vector3 direction;
    public Vector3 targetPoint;

    private bool done = false;
    private ParticleSystem blastInst = null;

	// Use this for initialization
	void Start () {
        toIgnore.Add(gameObject);
        timeFired = Time.time;
        blastInst = Instantiate(blast);
        blastInst.Play();
        ParticleSystem ex = Instantiate(spawn);
        ex.transform.position = gameObject.transform.position;
        ex.Play();
	}

    public void rePickTarget()
    {
        GameObject currentTarget = null;
        float currentDistance = 0;

        List<GameObject> possibleTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("ShootingDummy"));
        possibleTargets.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Car")));
        foreach (GameObject ign in toIgnore)
        {
            possibleTargets.Remove(ign);
        }
        foreach (GameObject possibleTarget in possibleTargets)
        {
            float distance = (gameObject.transform.position - possibleTarget.transform.position).magnitude;
            if (currentTarget == null || currentDistance > distance)
            {
                currentTarget = possibleTarget;
                currentDistance = distance;
            }
        }
        target = currentTarget;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (done)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (!audio.isPlaying)
            {
                Destroy(gameObject);
            }
            return;
        }
        float speedMod = 1;
        if (Time.time - timeFired > timeIdle)
        {
            if (target == null)
            {
                rePickTarget();
            }
            float distance = (gameObject.transform.position - target.transform.position).magnitude;
            float speedModifier = target.GetComponent<Rigidbody>().velocity.magnitude * distance * forwardModifier;
            targetPoint = target.transform.position + target.GetComponent<Rigidbody>().velocity * speedModifier;
            //Vector3 ownPredictedPosition = transform.position + gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 ownPredictedPosition = transform.position;
            //find the vector pointing from our position to the target
            direction = (targetPoint - ownPredictedPosition).normalized;

            //create the rotation we need to be in to look at the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            speedMod = (1 - rotationBasedSpeedMod) + rotationBasedSpeedMod * (180 - Quaternion.Angle(lookRotation, transform.rotation)) / 180;

            Vector3 currVel = gameObject.GetComponent<Rigidbody>().velocity;
            gameObject.GetComponent<Rigidbody>().AddForce(currVel*-brake*(1-speedMod), ForceMode.VelocityChange);
        };
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, maxSpeed*speedMod, ForceMode.VelocityChange);
        if (!(target == null) && (gameObject.transform.position - target.transform.position).magnitude < explosionDistance)
        {
            Explode();
        }
        if (Time.time - timeFired > timeIdle + maxFlightTime)
        {
            Explode();
        }
        blastInst.transform.position = gameObject.transform.position + gameObject.transform.rotation * (new Vector3(0,0,-1.5f));
        blastInst.transform.rotation = gameObject.transform.rotation;
	}

    void Explode()
    {
        if (debug_neverExplode || done)
        {
            return;
        }
        AudioSource audio = GetComponent<AudioSource>();
        audio.pitch = Random.Range(1f, 2f);
        audio.Play();
        audio.Play(44100);
        done = true;
        ParticleSystem ex = Instantiate(explosion);
        ex.transform.position = gameObject.transform.position;
        ex.Play();
        List<GameObject> possibleTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("ShootingDummy"));
        possibleTargets.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Car")));
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if ((transform.position - possibleTarget.transform.position).magnitude > explosionRadius)
            {
                continue;
            }
            Driving dr = possibleTarget.GetComponent<Driving>();
            if (dr != null)
            {
                dr.canDrive = false;
                possibleTarget.GetComponent<PowerupHandler>().addDelayedBehavior(new DelayedControlReset().withTimer(1).forScript(dr));
            }
            possibleTarget.GetComponent<Rigidbody>().AddExplosionForce(explosionForce*10, transform.position + explosionCorrection, explosionRadius);
        }
        blastInst.Stop();
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnCollisionEnter(Collision col)
    {
        Explode();
    }
}
