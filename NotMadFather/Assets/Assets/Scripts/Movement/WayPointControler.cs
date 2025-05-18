/*
*   LOOPS through a list of waypoints for movement
*
*   Sets starting pos as first waypoint
*/

using UnityEngine;

public class WaypointController : TopDownController
{
    public Transform[] waypoints;
    private int currentWaypointIndex;

    [Header("Enemy Settings")]
    public bool isEnemy;
    public float detectionRange = 2f;
    public LayerMask playerLayer;
    public LayerMask obstructionMask;

    private Transform player;
    private bool isChasing = false;
    private float lostTimer = 0f;
    public float lostDuration = 2f; // how long to chase without seeing YOU :D Get scared Woooooo
    [Header("Movement Speed")]
    public float patrolSpeed = 1f; // Our Normal movement speed
    public float chaseSpeedMultiplier = 2f; // Basically if we see the player, times out speed by this number

    void Awake()
    {
        currentWaypointIndex = 0;
        transform.position = waypoints[currentWaypointIndex].transform.position;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (GetComponent<Dangerous>() != null) // has the enemy script? then its an enemy
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
        }
    }

    void Update()
    {
        if (isEnemy && player != null)
        {
            if (CanSeePlayer() && !Manager.Instance.medication) // only chase the player in withdrawl world
                {
                    isChasing = true;
                    lostTimer = 0f;
                }
                else if (isChasing)
                {
                    lostTimer += Time.deltaTime;
                    if (lostTimer >= lostDuration)
                    {
                        isChasing = false; // lost the player
                        lostTimer = 0f;
                    }
                }
        }

        Vector2 movement = isChasing ? FindChaseMovement() : FindMovement();

        if (!isChasing)
        {
            CheckWaypoint();
        }
        moveSpeed = isChasing ? patrolSpeed * chaseSpeedMultiplier : patrolSpeed;

        base.CalculateMovement(movement);
    }

    private void CheckWaypoint()
    {
        if (waypoints.Length > 0)
        {
            float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);

            if (distanceToWaypoint < 0.5f) // if reached the next waypoint, move on to next
            {
                currentWaypointIndex  = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private Vector2 FindMovement()
    {
        Vector2 movement = Vector2.zero;

        if (waypoints.Length > 0)
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = waypoints[currentWaypointIndex].position;

            Vector2 direction = targetPosition - currentPosition;

            if (Mathf.Abs(direction.x) > 0.1f)
                movement.x = Mathf.Sign(direction.x);

            if (Mathf.Abs(direction.y) > 0.1f)
                movement.y = Mathf.Sign(direction.y);
        }

        return movement;
    }

    private Vector2 FindChaseMovement()
    {
        Vector2 movement = Vector2.zero;
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.x) > 0.1f)
            movement.x = Mathf.Sign(direction.x);

        if (Mathf.Abs(direction.y) > 0.1f)
            movement.y = Mathf.Sign(direction.y);

        return movement;
    }

    private bool CanSeePlayer()
    {
        Vector2 dirToPlayer = player.position - transform.position;

        if (dirToPlayer.magnitude <= detectionRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, detectionRange, playerLayer | obstructionMask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (isEnemy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }

}
