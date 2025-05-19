using System;
using UnityEngine;

public class NPC : SwitchableSprite, IDialogueObject
{
    private bool recentlyFinishedDialogue = false;
    private float dialogueResetCooldown = 0.2f; // 0.2 seconds delay
    private float dialogueResetTimer = 0f;

    [Header("Dialogue")]
    [SerializeField] private string[] dialogue;

    private bool playerInRange = false;

    public bool RecentlyFinished => recentlyFinishedDialogue;

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

        if (playerInRange && Input.GetKeyDown(KeyCode.E)
            && !DialogueManager.Instance.IsDialogueActive()
            && !recentlyFinishedDialogue)
        {
            Speak();
        }

        Debug.Log("Player in range is " + playerInRange);
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

    public void Speak()
    {
        if (!DialogueManager.Instance.IsDialogueActive())
        {
            DialogueManager.Instance.StartDialogue(dialogue, this);
        }
    }

    public void UpdateDialogue(string[] newDialogue)
    {
        dialogue = newDialogue;
    }

    public string[] Dialogue => dialogue;

    public void OnDialogueFinished()
    {
        Debug.Log($"{gameObject.name} finished dialogue.");
        recentlyFinishedDialogue = true;
    }

}
