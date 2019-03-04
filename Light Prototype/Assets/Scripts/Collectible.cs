using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    //public Text Collectibles;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.collectiblesCollected++;
            FindObjectOfType<Player>().lightAmt += 2;
            Destroy(this.gameObject);
        }
    }
}
