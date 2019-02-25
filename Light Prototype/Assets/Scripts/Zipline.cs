using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    
    public bool isActive;
    public float anchorVisionRadius;
    private float currentVisionRadius = 0;
    
    public Transform AnchorA;
    public Transform AnchorB;

    public GameObject AnchorAMask;
    public GameObject AnchorBMask;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    void Start()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0,AnchorA.transform.position);
        _lineRenderer.SetPosition(1,AnchorB.transform.position);
    }

    void Update()
    {
        if (isActive)
        {
            if (currentVisionRadius < anchorVisionRadius - 0.1f)
            {
                currentVisionRadius += (anchorVisionRadius - currentVisionRadius) * 0.8f * Time.deltaTime;
                AnchorAMask.transform.localScale = new Vector3(currentVisionRadius, currentVisionRadius, 1);
                AnchorBMask.transform.localScale = new Vector3(currentVisionRadius, currentVisionRadius, 1);
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        AnchorAMask.SetActive(true);
        AnchorBMask.SetActive(true);
    }
}
