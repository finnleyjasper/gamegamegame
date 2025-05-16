using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // <-- Changed from string to ItemData
    public AudioClip pickupSound;

    private bool playerInRange = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.Instance.AddItem(itemData); // <-- Now passes ItemData, it HATED the old method (:)
            if (pickupSound != null && audioSource != null)
            {
                UIHint.Instance.ShowOutcome(this.gameObject, true);
                audioSource.PlayOneShot(pickupSound);
            }

            Destroy(gameObject, 0.4f); // Delay destruction for sound
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UIHint.Instance.ShowHint(true, this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UIHint.Instance.ShowHint(false, this.gameObject);
        }
    }
}
