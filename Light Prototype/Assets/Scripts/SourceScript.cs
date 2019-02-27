using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceScript : MonoBehaviour
{
    public GameObject visionMask;
    private CircleCollider2D coll;
    
    public float lightStored;

    private void Start()
    {
        coll = this.GetComponent<CircleCollider2D>();
        visionMask.transform.localScale = new Vector3(2, 2, 1);
    }

}
