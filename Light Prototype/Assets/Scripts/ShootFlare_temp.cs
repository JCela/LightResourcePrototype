using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFlare_temp : MonoBehaviour
{
    public GameObject FlareToThrow;
    
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (FlareToThrow != null)
                Instantiate(FlareToThrow, this.transform.position, Quaternion.identity);
        }
    }
}
