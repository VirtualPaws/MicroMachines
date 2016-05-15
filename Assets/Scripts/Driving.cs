using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour {
    //TODO: Smooth out/actually implement good vertical movement for ramps

    public float speed = 90f;
    public float turn = 5f;
    public float hoverforce = 65f;
    public float hoverHeight = 3.5f;
    
    public float angularGrip = 0.985f;
    public float speedGrip = 0.7f;

    private float powerInput;
    private float turnInput;
    //Threshholds for a total stop if its slower than this and no inputs are made
    public float angleThreshhold = 0.5f;
    public float speedThreshhold = 0.2f;
    private Rigidbody carRigidbody;

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

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;

            //Reduce floating in the air here/TODO: apply pressure from the top if over threshhold value
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverforce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.VelocityChange);
        }

        //add inputs
        carRigidbody.AddRelativeTorque(0, turnInput * turn, 0f);
        carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed);
        
        
        //Velocity vector correction/always move forward, needs to be a transform instead of a declaration for impacts 
        carRigidbody.velocity = transform.forward * carRigidbody.velocity.magnitude;

        

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
