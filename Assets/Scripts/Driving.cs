using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour {
    //TODO: implement separate physiclayer for raycasts to increase performance
    //TODO: improve Velocity computing

    public float speed = 60f;
    public float turn = 1.5f;
    //deprecated, used in hoverphysics
    //public float hoverforce = 100f;
    public float checkHeight = 100f;
    
    //Velocity of this Object is 2 parts: internal velocity, applied through controls and external velocity, applied through collision
    //internal Velocity is alway pointed forward and the total velocity is the sum of internal and external influences
    private Vector3 extVelocity = new Vector3(0, 0, 0);
    private Vector3 intVelocity = new Vector3(0, 0, 0);

    public float angularGrip = 0.7f;
    public float speedGrip = 0.9f;

    private float powerInput;
    private float turnInput;
    //Threshholds for a total stop if its slower than this and no inputs are made
    public float angleThreshhold = 0.2f;
    public float speedThreshhold = 0.2f;
    private Rigidbody carRigidbody;

    void onCollisionEnter(Collision col)
    {
        //change velocityvector
    }

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        powerInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
	}

    void FixedUpdate()
    {
        //Ray gets shot down to keep stable position on the road
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, checkHeight))
        {
            /* Apply physical force, kinda like a hovercar
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverforce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.VelocityChange);
             */

            //stick car to the floor
            Vector3 position = hit.point;
            position.y += 1f;

            transform.position = position;
        }

        //add inputs
        carRigidbody.AddRelativeTorque(0, turnInput * turn, 0f);
        carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
        
        
        //compute velocity, blockiert momentan den rückwärtsgang
        //needs to be improved
        intVelocity = transform.forward * carRigidbody.velocity.magnitude;
        carRigidbody.velocity = intVelocity + extVelocity; 

        

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
        
        if(carRigidbody.velocity.magnitude > 0)
        {
            if (powerInput == 0)
            {
                carRigidbody.velocity *= speedGrip;
                if (carRigidbody.velocity.magnitude < speedThreshhold)
                {
                    carRigidbody.velocity = new Vector3(0, 0, 0);
                }
            }
        }
        
    }
}
