using UnityEngine;
using System.Collections;

public class CarCollision : MonoBehaviour {
    
    public float forceMult = 1;

    private Rigidbody thisRigidBody;
    int layermask = 1 << 10; // players
	// Use this for initialization
	void Awake () {
        thisRigidBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
	
	}

    
    
    void OnTriggerEnter(Collider col)
    {
        //check if collided object is player
        if (col.gameObject.layer == 10)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool ishit = Physics.Raycast(ray, out hit, layermask);
            if(ishit)
            {
                Rigidbody otherPlayer = col.gameObject.GetComponent<Rigidbody>();
                Vector3 relVel = thisRigidBody.velocity - otherPlayer.velocity;

                otherPlayer.AddForceAtPosition(relVel*forceMult, hit.point); 
                Debug.Log("Player hit");

            }
            Debug.Log(ishit);
        }
    }
}
