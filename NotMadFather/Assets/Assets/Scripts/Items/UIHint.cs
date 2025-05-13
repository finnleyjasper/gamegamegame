using TMPro;
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
        string text = "Press E to ";

        if (caller.tag == "Item")
        {
            text += "pick up";
        }
        else if (caller.tag == "NPC")
        {
            text += "speak to";
        }

        hintUI.text = text;
        hintUI.gameObject.SetActive(show);
    }
}
