using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    
    //Light Stuff
    public bool lit;
    [Range(0, 10)]
    public float lightAmt;
    [Range(0, 10)]
    public float startingLightAmt;
    public float lightLostPerSecond;
    public float lightDetectionMulti, lightVisionMulti;
    private float lightAmtVisualized;
    [Range(0,1)]
    public float lightAmtVSpeed;
    private float detectionRadius;
    private float visionRadius;

    public Transform detectionMask;
    public Transform visionMask;

    public float interactRadius;

    public Sprite unlitSprite, litSprite;
    
    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider.radius = interactRadius;
    }

    // Update is called once per frame
    void Update()
    {
        //Reduce light every second
        if (lightAmt > 0)
        {
            lightAmt -= lightLostPerSecond * Time.deltaTime;
            if (lightAmt <= 0)
            {
                //When light runs out, reset variables and color
                lit = false;
                _collider.radius = interactRadius;
                detectionMask.gameObject.SetActive(false);
                visionMask.gameObject.SetActive(false);
                if (unlitSprite != null)
                {
                    _spriteRenderer.sprite = unlitSprite;
                }
                else
                {
                    _spriteRenderer.color = new Color32(150, 150, 150, 255);
                }
            }
        }
        
        //Move visualization towards actual light amount based on speed
        lightAmtVisualized += (lightAmt - lightAmtVisualized) * lightAmtVSpeed * Time.deltaTime;
        
        //Set detection and vision mask radiuses
        detectionRadius = lightAmtVisualized * lightDetectionMulti;
        visionRadius = lightAmtVisualized * lightVisionMulti;
        detectionMask.localScale = new Vector3(detectionRadius, detectionRadius, 1);
        visionMask.localScale = new Vector3(visionRadius, visionRadius, 1);
    }

    public void LightTorch()
    {
        //When lit, change variables and color
        lit = true;
        _collider.radius = 0;
        lightAmt = startingLightAmt;
        detectionMask.gameObject.SetActive(true);
        visionMask.gameObject.SetActive(true);
        if (litSprite != null)
        {
            _spriteRenderer.sprite = litSprite;
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }
}
