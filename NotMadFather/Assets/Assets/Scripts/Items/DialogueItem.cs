
using UnityEngine;

public class DialogueItem : MonoBehaviour, IDialogueObject
{
    private bool recentlyFinishedDialogue = false;
    private float dialogueResetCooldown = 0.2f; // 0.2 seconds delay
    private float dialogueResetTimer = 0f;
    public bool RecentlyFinished => recentlyFinishedDialogue;

    private bool playerInZone = false;
    [SerializeField] string[] comment;

    void Update()
    {
        if (recentlyFinishedDialogue)
        {
            dialogueResetTimer += Time.unscaledDeltaTime;
            if (dialogueResetTimer >= dialogueResetCooldown)
            {
                recentlyFinishedDialogue = false;
                dialogueResetTimer = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.E)
        && playerInZone && Manager.Instance.state == Manager.GameState.Playable
        && !DialogueManager.Instance.IsDialogueActive()
        && !recentlyFinishedDialogue)
        {
            Speak();
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
            UIHint.Instance.ShowHint(false, this.gameObject);
            playerInZone = false;
        }
    }

    public void Speak()
    {
        if (!DialogueManager.Instance.IsDialogueActive())
        {
            DialogueManager.Instance.StartDialogue(comment, this, Name());
        }
    }

    public void UpdateDialogue(string[] newDialogue)
    {
        comment = newDialogue;
    }

    public void OnDialogueFinished()
    {
        recentlyFinishedDialogue = true;
    }

    public string Name()
    {
        return "You"; // the one "speaking" when interacting with these objects will alwyas be the PC
    }
}
