using UnityEngine;

public class Door : InteractableItem
{
    private bool deleteScheduled = false;

    void Update()
    {
        ShouldBeInteractedWith();
        if (interactedWith && !deleteScheduled)
        {
            Invoke("Remove", 0.41f);
            deleteScheduled = true;
        }
    }

    void Remove()
    {
        gameObject.SetActive(false);
    }
}
