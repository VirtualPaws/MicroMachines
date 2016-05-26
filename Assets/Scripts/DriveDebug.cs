using UnityEngine;
using System.Collections;

public class DriveDebug : MonoBehaviour
{
    //TODO: implement separate physiclayer for raycasts to increase performance

    public float speed = 150f;
    public float turn = 1.5f;
    //distance the ray travels to check for floor, the lower, the better the performance
    public float checkHeight = 100f;

    public float angularGrip = 0.7f;
    public float speedGrip = 0.9f;

    //controls speed of descend in the air
    public float gravity = 0.5f;

    private float powerInput;
    private float turnInput;
    //Threshholds for a total stop if its slower than this and no inputs are made
    public float angleThreshhold = 0.2f;
    public float speedThreshhold = 0.2f;


    private Rigidbody carRigidbody;
    private float fallingspeed = 0;
    //Physics Layer shenanigans
    int layerMask = 1 << 8; //Layer 8 = Groundstuff

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Values between 0 and 1
        powerInput = 0;
        turnInput = 0;
    }

    void FixedUpdate()
    {
        //Ray gets shot down to keep stable position on the road
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, checkHeight, layerMask))
        {
            //stick car to the floor
            Vector3 position = hit.point;
            position.y += 1f;
            if (transform.position.y < position.y)
            {
                transform.position = position;
                fallingspeed = 0;
            }
            else if (transform.position.y > position.y)
            {
                fallingspeed += gravity;
                if (transform.position.y - fallingspeed > position.y)
                {
                    position.y = transform.position.y - fallingspeed;
                    transform.position = position;
                }
            }
        }

        //add inputs
        carRigidbody.AddRelativeTorque(0, turnInput * turn, 0f);
        carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);


        //grip, so car doesnt spin out of control, 
        if (carRigidbody.angularVelocity.magnitude > 0)
        {
            if (turnInput == 0)
            {
                carRigidbody.angularVelocity *= angularGrip;
                if (carRigidbody.angularVelocity.magnitude < angleThreshhold)
                {
                    carRigidbody.angularVelocity = new Vector3(0, 0, 0);
                }
            }
        }

        if (carRigidbody.velocity.magnitude > 0)
        {
            carRigidbody.velocity *= speedGrip;
            if (carRigidbody.velocity.magnitude < speedThreshhold)
            {
                carRigidbody.velocity = new Vector3(0, 0, 0);
            }
        }

    }
}
