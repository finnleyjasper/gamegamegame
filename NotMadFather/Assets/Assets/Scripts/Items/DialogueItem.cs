
using UnityEngine;

public class DialogueItem : MonoBehaviour, IDialogueObject
{
    private bool recentlyFinishedDialogue = false;
    private bool playerInZone = false;
    [SerializeField] string[] comment;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone && Manager.Instance.state != Manager.GameState.Playable)
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
            playerInZone = false;
        }
    }

    public void Speak()
    {
        if (!DialogueManager.Instance.IsDialogueActive())
        {
             DialogueManager.Instance.StartDialogue(comment, this);
        }
    }

    public void OnDialogueFinished()
    {
        recentlyFinishedDialogue = true;
    }
}
