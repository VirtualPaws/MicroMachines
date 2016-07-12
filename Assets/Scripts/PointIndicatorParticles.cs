using UnityEngine;
using System.Collections;

public class PointIndicatorParticles : MonoBehaviour {

    public ParticleSystem awardParticles;
    public ParticleSystem penaltyParticles;

    public void fireAwardParticles()
    {
        ParticleSystem ex = GameObject.Instantiate(awardParticles);
        ex.transform.position = transform.position;
        ex.transform.rotation = transform.rotation;
        //ex.transform.Rotate(new Vector3(0, 1, 0), 180);

        ex.Play();
    }

    public void firePenaltyParticles()
    {
        ParticleSystem ex = GameObject.Instantiate(penaltyParticles);
        ex.transform.position = transform.position;
        ex.transform.rotation = transform.rotation;
        //ex.transform.Rotate(new Vector3(0, 1, 0), 180);

        ex.Play();
    }
}
