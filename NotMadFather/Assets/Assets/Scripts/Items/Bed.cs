using UnityEngine;

public class Bed : MonoBehaviour
{
    public bool canSleep = false; // player can end the day?

    void Update()
    {
        if (canSleep)
        {
            gameObject.GetComponent<DialogueItem>().enabled = false;
            Debug.Log("Player has gone to sleep");
        }

    }
}
