using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<string> inventory = new List<string>();
    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;

    private bool isOpen = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
            if (isOpen) UpdateInventoryUI();
        }
    }

    public void AddItem(string itemName)
    {
        inventory.Add(itemName);
        Debug.Log("Picked up: " + itemName);
    }

    void UpdateInventoryUI()
    {
        inventoryText.text = "Inventory:\n";
        foreach (var item in inventory)
        {
            inventoryText.text += "• " + item + "\n";
        }
    }
}
