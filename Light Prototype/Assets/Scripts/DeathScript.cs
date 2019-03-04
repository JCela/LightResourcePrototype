using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    private Player player;
    private bool isInTorchLight;
    private bool isLightPlayer;
    public Text gameOverText;
    public Text pressToRestart;
    private float deathTimer;
    public Text deathTimerText;
    
    
    void Start()
    {
        player = GetComponent<Player>();
        isInTorchLight = false;
        isLightPlayer = true;
    }

    void Update()
    {
        Debug.Log(isInTorchLight);
        
        
        if (isLightPlayer == false && isInTorchLight == false)
        {
            deathTimerText.text = deathTimer+"";
            deathTimerText.enabled = true;
            deathTimer = (deathTimer - 1)*Time.deltaTime;
            
            if (deathTimer < 0)
            {
                Death();
            }
    
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
        gameOverText.text = "GAME OVER";
        pressToRestart.text = "Press R to restart!";

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
