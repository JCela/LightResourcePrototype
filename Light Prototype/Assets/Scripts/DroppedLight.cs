using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedLight : MonoBehaviour
{
    [Range(0, 10)]
    public float lightAmt;
    public float lightLostPerSecond;
    public float lightDetectionMulti, lightVisionMulti;
    private float lightAmtVisualized;
    [Range(0,1)]
    public float lightAmtVSpeed;
    private float detectionRadius;
    private float visionRadius;
    
    public Transform detectionMask;
    public Transform visionMask;

    void Update()
    {
        if (lightAmt > 0)
        {
            lightAmt -= lightLostPerSecond * Time.deltaTime;
        }
        else
        {
            detectionMask.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this.gameObject, 5.0f);
        }
        
        //Move visualization towards actual light amount based on speed
        lightAmtVisualized += (lightAmt - lightAmtVisualized) * lightAmtVSpeed * Time.deltaTime;
        
        //Set detection and vision mask radiuses
        detectionRadius = lightAmtVisualized * lightDetectionMulti;
        visionRadius = lightAmtVisualized * lightVisionMulti;
        detectionMask.localScale = new Vector3(detectionRadius, detectionRadius, 1);
        visionMask.localScale = new Vector3(visionRadius, visionRadius, 1);
    }
}
