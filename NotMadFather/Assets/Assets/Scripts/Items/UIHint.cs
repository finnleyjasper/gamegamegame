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

        if (caller.tag == "Item")
        {
            PickupItem item = caller.GetComponent<PickupItem>();
            text += "E to pick up " + item.itemData.name;
        }
        else if (caller.tag == "NPC")
        {
            text += "E to speak to";
        }
        else if (caller.tag == "Interaction") // make sure an outcome message isnt showing
        {
            text += "SPACE to use an item";
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
            InteractSpot interact = caller.GetComponent<InteractSpot>();

            if (isSuccessful)
            {
                text = "You used " + interact.requiredItem.name;
            }
            else
            {
                text = "You don't have the right item equipped...";
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
}
