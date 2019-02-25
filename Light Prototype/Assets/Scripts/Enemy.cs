using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _collider;
    
    public float patrolSpeed; //speed when there is no target
    public float runSpeed; //speed when moving towards a target
    public float turnSpeed;

    public bool isPatrolling;
    public float timeToLoseAggro;
    private float timeInDark; //timer for when enemy will lose all aggro

    public Vector3 patrolTarget;
    
    public GameObject currentTarget;
    public List<GameObject> detectedLightSources = new List<GameObject>();

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        //Assign a patrol point when initiated
        GetNewPatrolTarget(3.0f);
    }

    void Update()
    {   
        if (detectedLightSources.Count > 0)
        {
            //If there are nearby light sources, set current target to nearest one
            //Reset timeInDark
            timeInDark = 0;
            isPatrolling = false;
            currentTarget = GetNearestLightSource();
        }
        else
        {
            //If there are no nearby light sources, add to timeInDark
            //If timeInDark reaches the timeToLoseAggro, set the currentTarget to null
            timeInDark += Time.deltaTime;
            if (timeInDark >= timeToLoseAggro)
            {
                currentTarget = null;
                if (!isPatrolling)
                {
                    isPatrolling = true;
                    GetNewPatrolTarget(3.0f);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!isPatrolling)
        {
            //If not patrolling, chase the target light source
            //Get the direction to the light source, rotate towards that direction, then move forward
            Vector3 directionToTarget = (currentTarget.transform.position - transform.position).normalized;
            Vector3 newRotation = Vector3.RotateTowards(transform.up, directionToTarget, turnSpeed * Time.fixedDeltaTime, 0.0f);
            transform.up = newRotation;
            
            _rigidBody.AddForce(transform.up * runSpeed * Time.fixedDeltaTime);
        }
        else if (isPatrolling)
        {
            //If we are patrolling, move towards the patrol point like above
            //If we are close to the target patrol point, get a new one
            Vector2 myPosV2 = transform.position;
            if (Vector2.Distance(myPosV2, patrolTarget) < 0.25f)
            {
                GetNewPatrolTarget(3.0f);
            }
            
            Vector3 directionToTarget = (patrolTarget - transform.position).normalized;
            Vector3 newRotation = Vector3.RotateTowards(transform.up, directionToTarget, turnSpeed * Time.fixedDeltaTime, 0.0f);
            transform.up = newRotation;
            
            _rigidBody.AddForce(transform.up * patrolSpeed * Time.fixedDeltaTime);
        }
    }
    
    //Same logic as GetNearestInteractable() from player script
    GameObject GetNearestLightSource()
    {
        if (detectedLightSources.Count > 0)
        {
            GameObject nearestLightSource= null;
            float closestDistance = 100;

            foreach (GameObject interactable in detectedLightSources)
            {
                Vector2 playerPos = this.transform.position;
                Vector2 intPos = interactable.transform.position;

                float distanceToPlayer = Vector2.Distance(playerPos, intPos);

                if (distanceToPlayer < closestDistance)
                {
                    nearestLightSource = interactable;
                    closestDistance = distanceToPlayer;
                }
            }
            return nearestLightSource;
        }
        else
        {
            return null;
        }
    }

    //Set patrol target to a random point within a certain radius circle
    void GetNewPatrolTarget(float radius)
    {
        Vector3 randomPoint = Random.insideUnitCircle * radius;
        patrolTarget = (randomPoint + transform.position);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            detectedLightSources.Add(other.gameObject);
        }
        
        if(other.CompareTag("Explosion"))
            Debug.Log("EXPLOSION!!!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            detectedLightSources.Remove(other.gameObject);
        }
    }
}
