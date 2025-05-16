using UnityEngine;

public class InteractSpot : MonoBehaviour
{
    public ItemData requiredItem;
    private bool interactedWith = false;
    private bool playerInZone = false; // this is a fucking weird way to do it but i was having issues with collisionenter/stay
    private PlayerInventory player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInZone)
            {
                if (player.equippedItem == requiredItem)
                {
                    interactedWith = true;
                    player.RemoveItem(requiredItem);
                    UIHint.Instance.ShowOutcome(this.gameObject, true);
                    // Destroy(this.gameObject) -- depends on the item? some will need to delete themselves
                }
                else
                {
                    UIHint.Instance.ShowOutcome(this.gameObject, false);
                }
            }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIHint.Instance.ShowHint(true, this.gameObject);
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInZone = false;
        }
    }
}

