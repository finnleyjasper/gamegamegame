using UnityEngine;

public class DialogueDoor : MonoBehaviour
{
    private Door door;
    private DialogueItem dialogueItem;
    private PlayerInventory playerInventory;

    void Start()
    {
        door = GetComponent<Door>();
        dialogueItem = GetComponent<DialogueItem>();
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();

        door.enabled = false;
        dialogueItem.enabled = true;
    }

    void Update()
    {
        if (playerInventory.equippedItem == door.requiredItem)
        {
            dialogueItem.enabled = false;
            gameObject.tag = "Interaction";
            door.enabled = true;
        }
        else
        {
            gameObject.tag = "Dialogue Item";
            dialogueItem.enabled = true;
            door.enabled = false;
        }
    }
}
