using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
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

    public GameObject ignitedSprite;
    public GameObject Explosion;

    public Color colorInit;
    public Color colorFinal;
    public float ColorLerpDuration = 3f;
    
    private Color lerpedColor = Color.white;

    private float colorTimer = 0;
    private bool flag;
    
    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider.radius = interactRadius;
        
        ignitedSprite.GetComponent<SpriteRenderer>().color = colorInit;
    }

    // Update is called once per frame
    void Update()
    {
        if (lit)
        {
            ignitedSprite.gameObject.transform.localScale = detectionMask.localScale;
            
            Color colorIgnite = Color.Lerp(colorInit, colorFinal, colorTimer);
            colorTimer += Time.deltaTime / ColorLerpDuration;
            ignitedSprite.GetComponent<SpriteRenderer>().color = colorIgnite;
        }

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

    public void Ignite()
    {
        lit = true;
        _collider.radius = 0;
        lightAmt = startingLightAmt;
        detectionMask.gameObject.SetActive(true);
        visionMask.gameObject.SetActive(true);

        StartCoroutine(igniteCo(ColorLerpDuration));

        // this.transform.localScale = visionMask.localScale;
    }

    private IEnumerator igniteCo(float timer)
    {
        yield return new WaitForSeconds(timer);
        Explosion.SetActive(true);
        Explosion.gameObject.transform.localScale = detectionMask.localScale;

        yield return new WaitForSeconds(0.25f);
        
        Destroy(this.gameObject);

    }
}
