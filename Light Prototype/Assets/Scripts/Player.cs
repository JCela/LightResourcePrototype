using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Components
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _collider;
    
    //Movement
    public float moveSpeed;
    private Vector2 inputVector;
    
    //Light
    [Header("Light")]
    public float lightAmt;
    public float lightLostPerSecond;
    public float lightDetectionMulti, lightVisionMulti;
    private float lightAmtVisualized; //the visualization of the lightAmt, used for visuals dampening
    [Range(0,1)]
    public float lightAmtVSpeed;
    private float detectionRadius; //distance where enemies will detect you
    private float visionRadius; //distance that player can see 

    public Transform detectionMask;
    public CircleCollider2D detectionCollider;
    public Transform visionMask;
   
    //Interactions
    [Header("Interactions")]
    public float torchCost; //light used when lighting a torch
    public float lightLostOnHit; //light lost when hit by an enemy
    public float knockbackPower; //power of force when hit by an enemy
    public GameObject droppedLightPrefab; //prefab for object dropped when hit by enemy

    public float bombCost;
    
    
    public float graceTimer;
    public float gracePeriod;
    
    public GameObject interactText;
    
    public List<GameObject> nearbyInteractables = new List<GameObject>(); //list containing all nearby interactables
    
    void Awake()
    {
        //assign component variables
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        //Reduce light every second and check for death
        if (lightAmt > 0)
        {
            lightAmt -= lightLostPerSecond * Time.deltaTime;
        }
        else
        {
            Death();
        }
        
        //Move visualization towards actual light amount based on speed
        lightAmtVisualized += (lightAmt - lightAmtVisualized) * lightAmtVSpeed * Time.deltaTime;
        
        //Set detection and vision mask radiuses
        detectionRadius = lightAmtVisualized * lightDetectionMulti;
        visionRadius = lightAmtVisualized * lightVisionMulti;
        detectionMask.localScale = new Vector3(detectionRadius, detectionRadius, 1);
        visionMask.localScale = new Vector3(visionRadius, visionRadius, 1);

        //If in grace period, disable the player's detection collider so enemies will chase the dropped light
        if (graceTimer <= 0)
        {
            detectionCollider.enabled = true;
        }
        else
        {
            graceTimer -= Time.deltaTime;
            detectionCollider.enabled = false;
        }

        //Handles interactions
        //If there is at least 1 object in the nearbyInteractables list, show indicator and allow player to press E to interact
        if (nearbyInteractables.Count > 0)
        {
            interactText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact(GetNearestInteractable());   
            }
        }
        else
        {
            interactText.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        //basic axis movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(horizontal, vertical).normalized;
        _rigidBody.AddForce(inputVector * moveSpeed * Time.fixedDeltaTime);
    }

    //Gets the nearest interactable within range
    GameObject GetNearestInteractable()
    {
        if (nearbyInteractables.Count > 0)
        {
            //temp variables to hold closest interactable and its distance from player
            GameObject nearestInteractable = null;
            float closestDistance = 100; //arbitrary large number

            foreach (GameObject interactable in nearbyInteractables)
            {
                //Get player and interactable positions and concert to Vector2
                Vector2 playerPos = this.transform.position;
                Vector2 intPos = interactable.transform.position;

                //Calculate distance to player
                float distanceToPlayer = Vector2.Distance(playerPos, intPos);

                //If distanceToPlayer is less than previous closest interactable, replace with new closest
                if (distanceToPlayer < closestDistance)
                {
                    nearestInteractable = interactable;
                    closestDistance = distanceToPlayer;
                }
            }
            return nearestInteractable;
        }
        else
        {
            return null;
        }
    }
    
    //Performs interaction with gameObject based on its tag
    //Executed when player is near interactables and presses "E"
    void Interact(GameObject interactable)
    {
        if (interactable != null)
        {
            //If interactable is a torch, check that we have enough light to activate it and that it is not already lit
            //If all conditions are met, light the torch, reduce lightAmt by cost
            if (interactable.CompareTag("Torch"))
            {
                TorchScript tScript = interactable.GetComponent<TorchScript>();
                if (tScript != null)
                {
                    if (lightAmt > torchCost && tScript.lit == false)
                    {
                        tScript.LightTorch();
                        lightAmt -= torchCost;
                    }
                }
            }
            //If interactable is a source, give player light from the source and destroy it
            else if (interactable.CompareTag("Source"))
            {
                SourceScript sScript = interactable.GetComponent<SourceScript>();
                if (sScript != null)
                {
                    lightAmt += sScript.lightStored;
                    Destroy(interactable);
                }
            }
            
            // If the interactable is a bomb, give the bomb some of player's light and ignite it
            else if (interactable.CompareTag("Bomb"))
            {
                Bomb bomb = interactable.GetComponent<Bomb>();
                if (bomb != null)
                {
                    lightAmt -= bombCost;
                    bomb.Ignite();
                }
            }
        }
    }

    //When lightAmt goes to 0
    void Death()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //When player collides with an enemy and player has not recently been hit
        //Start grace period
        //Instantiate dropped light
        //Push player and enemy away from each other
        if (other.gameObject.CompareTag("Enemy") && graceTimer <= 0)
        {
            lightAmt -= lightLostOnHit;
            graceTimer = gracePeriod;
            
            Instantiate(droppedLightPrefab, transform.position,Quaternion.identity);
            
            Vector3 knockbackDir = (transform.position - other.transform.position).normalized;
            _rigidBody.AddForce(knockbackDir * knockbackPower, ForceMode2D.Impulse);
            
            other.rigidbody.AddForce(knockbackDir * -1 * knockbackPower, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //When player enters interact collider of torch or source, add it to list of nearby interactables
        if (other.CompareTag("Torch") || other.CompareTag("Source") || other.CompareTag("Bomb"))
        {
            nearbyInteractables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Same as above, but removing it from the list
        if (other.CompareTag("Torch") || other.CompareTag("Source") || other.CompareTag("Bomb"))
        {
            nearbyInteractables.Remove(other.gameObject);
        }
    }
}
