using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    private WaypointTask waypointTask;

    void Start()
    {
        waypointTask = GetComponentInParent<WaypointTask>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (waypointTask.counter < 6)
        {
            waypointTask.counter++;
        }
    }
}
