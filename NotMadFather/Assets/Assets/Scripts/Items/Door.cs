using UnityEngine;

public class Door : InteractableItem
{
    private bool deleteScheduled = false;

    void Update()
    {
        CheckForDelete();
    }

    void CheckForDelete()
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
