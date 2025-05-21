using UnityEngine;

public class QTEInteractableItem : MonoBehaviour
{
    [Header("QTE Trigger")]
    public QTEManager qteManager;

    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.Space))
        {
            if (qteManager != null && !qteManager.IsActive)
            {
                qteManager.StartQTE();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIHint.Instance.ShowHint(true, this.gameObject);
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIHint.Instance.ShowHint(false, this.gameObject);
            playerInZone = false;
        }
    }
}
