using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    public float stallDuration;
    //public float shrinkTimer;
    public float speedOfBurst;
    public float maxSizeBurst;
    public float TimeTillDestroy;
    
    private float stallTimeRemaining;
    private float shrinkTimeRemaining;

    [Range(0, 10)]
    public float lightAmt;
    [Range(0,1)]
    public float lightAmtVSpeed;

    private float lightAmtVisualized;
    public float lightVisionMulti;
    
    
    public Transform visionMask;
    private float visionRadius;
    private float tempAlpha;

    public SpriteRenderer sr;
    
    private void Start()
    {
        stallTimeRemaining = stallDuration;
        shrinkTimeRemaining = stallDuration + 1.75f;
        tempAlpha = sr.color.a;
    }

    private void Update()
    {
        Debug.Log(lightAmt);

        //Debug.Log(tempAlpha);
        
        lightAmtVisualized += (lightAmt - lightAmtVisualized) * lightAmtVSpeed * Time.deltaTime;
        visionRadius = lightAmtVisualized * lightVisionMulti;
        visionMask.localScale = new Vector3(visionRadius, visionRadius, 1);

        
        // Timer until flare wil burst into large radius
        stallTimeRemaining -= Time.deltaTime;
        if (stallTimeRemaining < 0)
        {
            //StartCoroutine(TimeToDestroy(TimeTillDestroy));
            
            lightAmt += speedOfBurst*Time.deltaTime;
            if (lightAmt > maxSizeBurst)
                lightAmt = maxSizeBurst;
        }
    
        // Timer until flare will shrink after bursting
        shrinkTimeRemaining -= Time.deltaTime;
        if (shrinkTimeRemaining < 0)
        {
            StartCoroutine(TimeToDestroy(0.5f));

            lightAmt -= 8 * (speedOfBurst * Time.deltaTime);
            /*if (lightAmt < 0)
            {
                lightAmt = 0;
            }*/
            
            tempAlpha -= Time.deltaTime;
            sr.color = new Color(255, 203, 114, tempAlpha);

        }
    }

    private IEnumerator TimeToDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
