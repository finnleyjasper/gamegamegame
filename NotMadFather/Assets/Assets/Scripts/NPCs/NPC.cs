using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField] private string[] dialogue; // can copy paste text in the inspector
    private int dialogueIndex = 0;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(dialogue[dialogueIndex]);

            // goes through an npcs dialogue then loops the last line
            if (dialogue.Length > (dialogueIndex + 1))
                // this is where actual dialogue stuff goes...
                // ...we debug for now
                dialogueIndex ++;
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
