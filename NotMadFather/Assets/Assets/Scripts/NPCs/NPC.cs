using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum DialogueMode
    {
        PlayerInput, // player can press E to prompt dialogue
        ManagerController // the manager script schedules dialogue
    }

    private bool playerInRange = false;

    [SerializeField] private string[] dialogue; // can copy paste text in the inspector
    private int dialogueIndex = 0;
    public DialogueMode currentDialogueMode;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && currentDialogueMode == DialogueMode.PlayerInput)
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
        Debug.Log(dialogue[dialogueIndex]);

        // goes through an npcs dialogue then loops the last line
        if (dialogue.Length > (dialogueIndex + 1))
            // this is where actual dialogue stuff goes...
            // ...we debug for now
            dialogueIndex++;
    }

    public int DialogueIndex
    {
        get { return dialogueIndex; }
    }

    public string[] Dialogue
    {
        get { return dialogue; }
    }
}
