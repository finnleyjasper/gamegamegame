using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    private string[] currentDialogue;
    private int currentIndex;
    private bool dialogueActive;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextLine();
        }
    }

    private NPC currentSpeaker;

    public void StartDialogue(string[] dialogue, NPC npc)
    {
        currentDialogue = dialogue;
        currentIndex = 0;
        dialogueActive = true;
        currentSpeaker = npc;

        dialogueBox.SetActive(true);
        Time.timeScale = 0f;
        ShowNextLine();
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueActive = false;
        Time.timeScale = 1f;

        if (currentSpeaker != null)
        {
            currentSpeaker.OnDialogueFinished();
            currentSpeaker = null;
        }
    }

    private void ShowNextLine()
    {
        if (currentIndex < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[currentIndex];
            currentIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }
}
