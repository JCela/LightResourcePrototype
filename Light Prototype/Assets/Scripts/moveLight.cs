using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLight : MonoBehaviour
{
    //rigidbody of this light
    private Rigidbody2D lightBody;

    //the force at which the light is shot
    public float thrust = 800f;
    
    //assign 800 for thrust in inspector
    //linear drag = 1
    

    // when this object is instantiated
    // Shoot it forward
    void Start()
    {
        lightBody = GetComponent<Rigidbody2D>();
        lightBody.AddForce(transform.right * thrust);
    }

}
