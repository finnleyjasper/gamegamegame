using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
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
            PlayerInventory.Instance.AddItem(itemName);
            if (pickupSound != null && audioSource != null)
                audioSource.PlayOneShot(pickupSound);

            Destroy(gameObject, 0.1f); // Delay destruction for sound
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            PickupHint.Instance.ShowHint(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            PickupHint.Instance.ShowHint(false);
        }
    }
}
