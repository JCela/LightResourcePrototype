using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.collectiblesCollected++;
            Debug.Log("Collected: " + Player.collectiblesCollected);
            Destroy(this.gameObject);
        }
    }
}
