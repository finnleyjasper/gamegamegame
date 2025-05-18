using System;
using UnityEngine;

public class NPC : SwitchableSprite, IDialogueObject
{
    private bool recentlyFinishedDialogue = false;
    private float dialogueResetCooldown = 0.2f; // 0.2 seconds delay
    private float dialogueResetTimer = 0f;

    [Header("Dialogue")]
    [SerializeField] private string[] dialogue; // can copy paste text in the inspector

    private bool playerInRange = false;

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

    public void UpdateDialogue(string[] newDialogue) // give the npc a new set of dialogue
    {
        dialogue = newDialogue;
    }

    public string[] Dialogue
    {
        get { return dialogue; }
    }

    public void OnDialogueFinished()
    {
        recentlyFinishedDialogue = true;
    }

}
