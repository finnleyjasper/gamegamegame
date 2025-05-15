using UnityEngine;

public class InteractSpot : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem;
    private bool interactedWith = false;
    private PlayerInventory player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerInventory>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // removes the item after using
            if (Input.GetKeyDown(KeyCode.E) && !interactedWith)
            {
                if (player.equippedItem == requiredItem)
                {
                    interactedWith = true;
                    player.RemoveItem(requiredItem);
                    Debug.Log(gameObject.name + " has been interacted with, using " + requiredItem.name);
                    Destroy(this); // is this wrong?
                }
                else
                {
                    Debug.Log("You don't have the right item!");
                }
            }
        }
    }
}

// maybe make this like item with a scriptable thing and a prefab?????
