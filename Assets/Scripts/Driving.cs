using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour
{
    public float speed = 500f;
    //Tested for clipping at a about 45degree angle up to 1500 without problems at the current physicupdatesettings
    public float maxspeed = 700f;

    //about 50 if the turning should be really quick, 30 is a bit slower, everything under 29 seems unusable
    public float turn = 20f;
    public float maxturn = 50f;
    //distance the ray travels to check for floor, the lower, the better the performance
    public float checkHeight = 100f;

    //controls speed of descend in the air
    public float gravity = 0.1f;

    private float powerInput;
    private float turnInput;

    //Multiplayer
    //Valid Strings are "Keyboard", "Keyboard2", "Controller1" & "Controller2"
    public string inputDevice = "Keyboard";

    //Oil Powerup Timer
    private const float OILFRICT = 0.005f;
    private float oiltimer = 0f;
    private bool oily = false;
    public float oiltime = 1.0f;

    //Threshholds for a total stop if its slower than this and no inputs are made
    private float angleThreshhold = 0.2f;
    private float speedThreshhold = 0.2f;

    //Deprecated: Friction gets set through underlaying Physicsmaterial
    private float angularGrip = 0.8f;
    private float speedGrip = 0.8f;


    private Rigidbody carRigidbody;
    private Quaternion curRot;
    private float fallingspeed = 0.05f;
    //Physics Layer shenanigans
    int layerMask = 1 << 8; //Layer 8 = Groundstuff

    private bool stuckInWater = false;
    private bool drowning = false;
    private float drowntimer = 0f;
    private Vector2 safetyNet;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Values between 0 and 1
        switch (inputDevice)
        {
            case "Keyboard":
                powerInput = Input.GetAxis("Vertical1");
                turnInput = Input.GetAxis("Horizontal1");
                if (Input.GetButtonDown("Powerup1"))
                {
                    GetComponent<PowerupHandler>().firePowerUp();
                }
                break;
            case "Keyboard2":
                powerInput = Input.GetAxis("Vertical4");
                turnInput = Input.GetAxis("Horizontal4");
                if (Input.GetButtonDown("Powerup4"))
                {
                    GetComponent<PowerupHandler>().firePowerUp();
                }
                break;
            case "Controller1":
                powerInput = Input.GetAxis("Vertical2");
                turnInput = Input.GetAxis("Horizontal2");
                if (Input.GetButtonDown("Powerup2"))
                {
                    GetComponent<PowerupHandler>().firePowerUp();
                }
                break;
            case "Controller2":
                powerInput = Input.GetAxis("Vertical3");
                turnInput = Input.GetAxis("Horizontal3");
                if (Input.GetButtonDown("Powerup3"))
                {
                    GetComponent<PowerupHandler>().firePowerUp();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        castRayDown();
        if (!drowning)
        {
            updateVelocity();
        }
        else
        {
            carRigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (drowning)
        {
            drowntimer += Time.fixedDeltaTime;
            if (drowntimer > 1)
            {
                drowning = false;
                drowntimer = 0;
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

    //Everything that has to do with surface under the Car: Gravity, friction, oil, rotation
    void castRayDown()
    {
        //Ray gets shot down to keep stable position on the road
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        Vector3 position;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            //stick car to the floor
            position = hit.point;
            position.y += 0.5f;
            if (drowning)
            {
                Vector2 net = new Vector2(transform.position.x, transform.position.z);
                if (!safetyNet.Equals(net))
                {
                    drowning = false;
                }
                else
                {
                    position = transform.position;
                    position.y -= 0.05f;
                    transform.position = position;
                    safetyNet = net;
                }
            }
            else if (transform.position.y < position.y)
            {
                fallingspeed = 0;
                transform.position = position;
            }
            else if (transform.position.y > position.y)
            {
                fallingspeed += gravity;
                if (transform.position.y - fallingspeed > position.y)
                {
                    position.y = transform.position.y - fallingspeed;
                    transform.position = position;
                }
                else if (stuckInWater)
                {
                    drowning = true;
                    safetyNet = new Vector2(transform.position.x, transform.position.z);
                }
            }

            //Check for physics material to update friction .. 1- so that the friction-value in editor makes sense
            if (hit.collider.material)
            {
                if (hit.transform.tag == "Oil")
                {
                    oily = true;
                    speedGrip = 1 - OILFRICT;
                    oiltimer = 0f;
                }

                if (hit.transform.tag == "Water")
                {
                    stuckInWater = true;
                }
                else
                {
                    stuckInWater = false;
                }

                if (!oily)
                {
                    speedGrip = 1 - hit.collider.material.dynamicFriction / 5;
                }
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

    }

    void updateVelocity()
    {

        //Beschleunigungskräfte anwenden
        if (carRigidbody.velocity.magnitude > 1)
        {
            carRigidbody.AddRelativeTorque(0, turnInput * turn, 0f);
            if (carRigidbody.angularVelocity.magnitude > maxturn)
            {
                carRigidbody.angularVelocity = carRigidbody.angularVelocity.normalized * maxturn;
            }
        }

        if (!oily)
        {
            carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
            if (carRigidbody.velocity.magnitude > maxspeed)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * maxspeed;
            }
        }
        else
        {
            carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed / 5f);
            if (carRigidbody.velocity.magnitude > maxspeed)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * maxspeed;
            }
        }

        //grip, so car doesnt spin out of control, 
        if (carRigidbody.angularVelocity.magnitude > 0)
        {

            carRigidbody.angularVelocity *= angularGrip;


            if (carRigidbody.angularVelocity.magnitude < angleThreshhold)
            {
                carRigidbody.angularVelocity = new Vector3(0, 0, 0);
            }
            //carRigidbody.angularVelocity = carRigidbody.angularVelocity.normalized*((carRigidbody.velocity.magnitude*carRigidbody.angularVelocity.magnitude)/maxspeed);
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
