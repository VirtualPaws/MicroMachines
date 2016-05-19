using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour {
    //TODO: implement separate physiclayer for raycasts to increase performance
    
    public float speed = 150f;
    public float turn = 1.5f;
    //distance the ray travels to check for floor, the lower, the better the performance
    public float checkHeight = 100f;
    
    public float angularGrip = 0.7f;
    public float speedGrip = 0.9f;

    private float powerInput;
    private float turnInput;
    //Threshholds for a total stop if its slower than this and no inputs are made
    public float angleThreshhold = 0.2f;
    public float speedThreshhold = 0.2f;
    private Rigidbody carRigidbody;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
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
            //stick car to the floor
            Vector3 position = hit.point;
            position.y += 1f;

            transform.position = position;
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
        
        if(carRigidbody.velocity.magnitude > 0)
        {
            carRigidbody.velocity *= speedGrip;
            if (carRigidbody.velocity.magnitude < speedThreshhold)
            {
                carRigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
        
    }
}
