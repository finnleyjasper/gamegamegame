using System.Linq;
using UnityEngine;

public class QTEInteractableItem : DialogueItem
{
    [Header("QTE Trigger")]
    public QTEManager qteManager;
    public bool successful;

    void Update()
    {
        if (playerInZone)
        {
            if (qteManager != null && !qteManager.IsActive && Input.GetKeyDown(KeyCode.Space) && !DialogueManager.Instance.IsDialogueActive())
            {
                qteManager.StartQTE(this);
            }
        }
    }
}
