using UnityEngine;
using System.Collections;

public class CarCollision : MonoBehaviour
{

    public float forceMult = 1;

    private Rigidbody thisRigidBody;

    void Awake()
    {
        thisRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

    }



    void OnTriggerEnter(Collider col)
    {
        //check if collided object is player
        if (col.gameObject.layer == 10)
        {
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);
            RaycastHit hit;
            //Ray forward
            bool ishit = col.Raycast(ray, out hit, 5f);
            if (ishit)
            {
                Debug.Log("Test");
                Rigidbody otherPlayer = col.gameObject.GetComponent<Rigidbody>();
                //compute forward velocity
                float tempVel = transform.InverseTransformDirection(thisRigidBody.velocity).z;
                Vector3 dirVel = transform.forward * tempVel;

                otherPlayer.AddForceAtPosition(dirVel * forceMult, hit.point);

            }
            else
            {
                //Ray right side
                ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.right);
                ishit = col.Raycast(ray, out hit, 5f);
                if (ishit)
                {
                    Rigidbody otherPlayer = col.gameObject.GetComponent<Rigidbody>();
                    //compute sideways velocity
                    float tempVel = transform.InverseTransformDirection(thisRigidBody.velocity).x;
                    Vector3 dirVel = transform.forward * tempVel;

                    otherPlayer.AddForceAtPosition(dirVel * forceMult, hit.point);
                }
                else
                {
                    //Ray left side
                    ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), -transform.right);
                    ishit = col.Raycast(ray, out hit, 5f);
                    if (ishit)
                    {
                        Rigidbody otherPlayer = col.gameObject.GetComponent<Rigidbody>();
                        //compute sideways velocity
                        float tempVel = transform.InverseTransformDirection(thisRigidBody.velocity).x;
                        Vector3 dirVel = transform.forward * tempVel;

                        otherPlayer.AddForceAtPosition(dirVel * forceMult, hit.point);
                    }
                }
            }
        }
    }
}
