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

    void Awake()
    {
        transform.position = waypoints[currentWaypointIndex].transform.position;
    }

    void Update()
    {
        CheckWaypoint();
        base.CalculateMovement(FindMovement());
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

        Debug.Log("Current waypoint: " + waypoints[currentWaypointIndex].gameObject.name);
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


}
