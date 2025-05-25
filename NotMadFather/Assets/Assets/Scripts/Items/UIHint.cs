using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHint : MonoBehaviour
{
    public static UIHint Instance;
    public TextMeshProUGUI hintUI;

    void Awake()
    {
        Instance = this;
        hintUI.gameObject.SetActive(false);
    }

    public void ShowHint(bool show, GameObject caller)
    {
        string text = "Press ";
        Debug.Log("caller: " + caller.name);

        if (caller.tag == "Item")
        {
            PickupItem item = caller.GetComponent<PickupItem>();
            text += "E to pick up " + item.itemData.name;
        }
        else if (caller.tag == "Dialogue Item")
        {
            text += "E to inspect";
        }
        else if (caller.tag == "NPC")
        {
            text += "E to speak to";
        }
        else if (caller.tag == "Interaction")
        {
            text += "SPACE to use item";
        }
        else if (caller.tag == "QTE")
        {
            text += "SPACE to complete test";
        }

        hintUI.text = text;
        hintUI.gameObject.SetActive(show);
    }

    public void ShowOutcome(GameObject caller, bool isSuccessful) // for interaction spots, item pickups
    {
        string text = "";

        if (caller.tag == "Item")
        {
            PickupItem item = caller.GetComponent<PickupItem>();
            text = "You picked up " + item.itemData.name;
        }
        else if (caller.tag == "Interaction")
        {
            InteractableItem interact = caller.GetComponent<InteractableItem>();

            if (isSuccessful)
            {
                text = "You used " + interact.requiredItem.name;
            }
            else
            {
                text = "That didn't work...";
            }
        }

        hintUI.text = text;
        hintUI.gameObject.SetActive(true);
        StartCoroutine(HideHint());
    }

    private IEnumerator HideHint()
    {
        yield return new WaitForSeconds(3f);
        hintUI.text = "";
        hintUI.gameObject.SetActive(false);
    }

    public void UpdateFont(TMP_FontAsset font)
    {
        hintUI.font = font;
    }
}
