using UnityEngine;

public class PickupHint : MonoBehaviour
{
    public static PickupHint Instance;
    public GameObject hintUI;

    void Awake()
    {
        Instance = this;
        hintUI.SetActive(false);
    }

    public void ShowHint(bool show)
    {
        hintUI.SetActive(show);
    }
}
