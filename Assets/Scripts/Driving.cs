using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 600f;
    public float turn = 5f;
=======
    public float speed = 500f;
    public float turn = 3f;
>>>>>>> a333d8b13ae12204280ce4240431b66647b47ba2
    //distance the ray travels to check for floor, the lower, the better the performance
    public float checkHeight = 100f;

    //controls speed of descend in the air
    public float gravity = 0.5f;

    private float powerInput;
    private float turnInput;
    
    //Oil Powerup Timer
    private const float OILFRICT = 0.9f;
    private float oiltimer = 0f;
    private bool oily = false;
    public float oiltime = 1.0f;

    //Threshholds for a total stop if its slower than this and no inputs are made
    private float angleThreshhold = 0.2f;
    private float speedThreshhold = 0.2f;

    //Deprecated: Friction gets set through underlaying Physicsmaterial
    private float angularGrip = 0.7f;
    private float speedGrip = 0.9f;


    private Rigidbody carRigidbody;
    private Quaternion curRot;
    private float fallingspeed = 0.05f;
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
        //TODO: Config Inputmanager und nutze Inputs für verschiedene Joysticks
        powerInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        //Ray gets shot down to keep stable position on the road
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        Vector3 position;

        if (Physics.Raycast(ray, out hit, checkHeight, layerMask))
        {
            //stick car to the floor
            position = hit.point;
            position.y += 0.5f;
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

            //Check for physics material to update friction .. 1- so that the friction in editor makes sense
            if (hit.collider.material)
            {
                if (!oily)
                {
                    speedGrip = 1 - hit.collider.material.dynamicFriction;
                }
                if (hit.transform.tag == "Oil")
                {
                    oily = true;
                    speedGrip = 1-OILFRICT;
                }
                angularGrip = (1 - hit.collider.material.dynamicFriction) * 0.9f;
            }

            //Car Rotation 
            //-- Rotation der X und Z Achse der unterliegenden Geometrie um die Y Rotation des Spielers transformieren und anwenden
            Quaternion newrot = hit.transform.rotation * Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0));
            if (this.transform.eulerAngles.x != newrot.eulerAngles.x || this.transform.eulerAngles.z != newrot.eulerAngles.z)
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(newrot.eulerAngles.x, this.transform.eulerAngles.y, newrot.eulerAngles.z), Time.deltaTime * 10.0f);
                curRot = hit.transform.rotation;
            }
        }
        else
        {
            fallingspeed += gravity;
            position = transform.position;
            position.y = transform.position.y - fallingspeed;
            transform.position = position;
        }

        //add inputs TODO: rotation nur wenn velocity>0 mach rotaton von velocity abhängig
        if (carRigidbody.velocity.magnitude > 0)
        {
            carRigidbody.AddRelativeTorque(0, turnInput * turn, 0f);
        }
        if (!oily)
        {
            carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
        }


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

        if (oily)
        {
            oiltimer += Time.fixedDeltaTime;
            if (oiltimer > oiltime)
            {
                oily = false;
            }
        }

    }
}
