using System;
using UnityEngine;

public class NPC : SwitchableSprite
{
    private bool recentlyFinishedDialogue = false;
    private float dialogueResetCooldown = 0.2f; // 0.2 seconds delay
    private float dialogueResetTimer = 0f;

    public enum DialogueMode
    {
        PlayerInput, // player can press E to prompt dialogue
        ManagerController // the manager script schedules dialogue
    }

    [Header("Dialogue")]
    [SerializeField] private string[] dialogue; // can copy paste text in the inspector
    public DialogueMode currentDialogueMode;

    private int dialogueIndex = 0;
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
            && currentDialogueMode == DialogueMode.PlayerInput
            && !DialogueManager.Instance.IsDialogueActive()
            && !recentlyFinishedDialogue)
        {
            Speak();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentDialogueMode == DialogueMode.PlayerInput)
        {
            playerInRange = true;
            UIHint.Instance.ShowHint(true, this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentDialogueMode == DialogueMode.PlayerInput)
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

    public int DialogueIndex
    {
        get { return dialogueIndex; }
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
