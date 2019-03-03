using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    
    public bool isLit = false;
    [Range(0, 10)]
    public float campLightAmt;
    [Range(0, 10)]
    public float campStartingLightAmt;
    public float campGoalAmt;
    public float campLightLostPerSecond;
    public float campDrainSpeed; //light drained/gained per second
    public float lightDetectionMulti, lightVisionMulti;
    private float lightAmtVisualized;
    
    [Range(0,1)]
    public float lightAmtVSpeed;
    private float detectionRadius;
    private float visionRadius;

    public Transform detectionMask;
    public Transform visionMask;

    public float interactRadius;

    public GameObject progressBar;
    
    public GameObject visionOverlay;
    
    public Sprite unlitSprite, litSprite;
    
    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider.radius = interactRadius;
    }
    
    void Update()
    {
        //Reduce light every second
        if (campLightAmt > 0)
        {
            campLightAmt -= campLightLostPerSecond * Time.deltaTime;
            if (campLightAmt <= 0)
            {
                //When light runs out, reset variables and color
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

        UpdateProgressBar();
        
        if (campLightAmt >= campGoalAmt)
        {
            GetLit();
        }
        
        
        //Move visualization towards actual light amount based on speed
        lightAmtVisualized += (campLightAmt - lightAmtVisualized) * lightAmtVSpeed * Time.deltaTime;
        
        
        detectionRadius = lightAmtVisualized * lightDetectionMulti;
        visionRadius = lightAmtVisualized * lightVisionMulti;
        detectionMask.localScale = new Vector3(detectionRadius, detectionRadius, 1);
        visionMask.localScale = new Vector3(visionRadius, visionRadius, 1);
    }
    
    public void LightCampfire()
    {
        _collider.radius = 0;
        campLightAmt = campStartingLightAmt;
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

    public void UpdateProgressBar()
    {
        RectTransform progressBarRT = progressBar.GetComponent<RectTransform>();
        progressBarRT.sizeDelta = new Vector2(2.0f * campLightAmt / campGoalAmt, progressBarRT.sizeDelta.y);
        progressBarRT.localPosition = new Vector3((progressBarRT.sizeDelta.x - 2.0f)/2, progressBarRT.localPosition.y, progressBarRT.localPosition.z );
    }

    public void GetLit()
    {
        gameObject.layer = 0;
        isLit = true;
        visionOverlay.SetActive(false);
        GameObject.Find("Player").GetComponent<Player>().nearbyInteractables.Remove(this.gameObject);
    }
}
