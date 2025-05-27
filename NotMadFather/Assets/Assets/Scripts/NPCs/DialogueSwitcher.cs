using UnityEngine;

public class DialogueSwitcher : MonoBehaviour
{
    [SerializeField] string[] normalDialogue;
    [SerializeField] string[] withdrawlDialogue;
    private string[] currentDialogue;


    void Start()
    {
        currentDialogue = normalDialogue;
    }

    void Update()
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
