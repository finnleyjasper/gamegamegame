using UnityEngine;

public class InteractSpot : SwitchableSprite
{
    [Header("Interaction Data")]
    public ItemData requiredItem;
    public bool interactedWith = false; // this will be used outside of this script

    private bool playerInZone = false; // this is a fucking weird way to do it but i was having issues with collisionenter/stay
    private PlayerInventory player;

    [Header("SFX")]
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip wrongItem;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerInventory>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInZone)
            {
                if (player.equippedItem == requiredItem)
                {
                    interactedWith = true;
                    audioSource.clip = success;
                    audioSource.Play();
                    player.RemoveItem(requiredItem);
                    UIHint.Instance.ShowOutcome(this.gameObject, true);
                    // Destroy(this.gameObject) -- depends on the item? some will need to delete themselves
                }
                else
                {
                    UIHint.Instance.ShowOutcome(this.gameObject, false);
                    audioSource.clip = wrongItem;
                    audioSource.Play();
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

