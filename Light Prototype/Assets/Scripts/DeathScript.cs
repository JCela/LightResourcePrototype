using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private Player player;
    private bool isInTorchLight;
    private bool isLightPlayer;
    
    
    void Start()
    {
        player = GetComponent<Player>();
        isInTorchLight = false;
        isLightPlayer = true;
    }

    void Update()
    {
        //Debug.Log(isInTorchLight);
        
        
        if (isLightPlayer == false && isInTorchLight == false)
        {
            Death();
        }

        if (player.lightAmt > 1)
        {
            isLightPlayer = true;
        }
        else if (player.lightAmt < 1  )
        {
            isLightPlayer = false;
        }
    }

    public void Death()
    {
        Debug.Log("DEAD");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            isInTorchLight = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            isInTorchLight = false;
        }
    }
}
