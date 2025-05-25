using UnityEngine;

public class DialogueSwitcher : MonoBehaviour
{
    [SerializeField] string[] normalDialogue;
    [SerializeField] string[] withdrawlDialogue;
    private string[] currentDialogue;

    private bool state;
    private bool previousState;

    void Start()
    {
        previousState = true;
        state = true;
        currentDialogue = normalDialogue;
    }

    void Update()
    {
        state = Manager.Instance.medication;
        if (state != previousState)
        {
            UpdateDialogue();
            previousState = state;
        }
    }

    private void UpdateDialogue()
    {
         if (Manager.Instance.medication)
            {
                currentDialogue = normalDialogue;
            }
            else
            {
                currentDialogue = withdrawlDialogue;
            }

        gameObject.GetComponent<NPC>().UpdateDialogue(currentDialogue);
    }
}
