

using UnityEngine;

public class WaypointTask : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    public int counter;
    public bool isComplete;

    void Update()
    {
        if (counter >= 6 && isComplete == false) // players needs to walk between points 3 times
        {
            isComplete = true;
        }

    }
}
