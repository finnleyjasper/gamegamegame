using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("UI Elements")]

    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI equippedItemText;

    [Header("Inventory")]
    public List<ItemData> items = new List<ItemData>();
    private int selectedIndex = -1;
    public ItemData equippedItem;

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
            if (isOpen)
                UpdateInventoryUI();
        }

        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (items.Count > 0)
                {
                    selectedIndex = (selectedIndex - 1 + items.Count) % items.Count;
                    UpdateInventoryUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (items.Count > 0)
                {
                    selectedIndex = (selectedIndex + 1) % items.Count;
                    UpdateInventoryUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && selectedIndex >= 0)
            {
                equippedItem = items[selectedIndex];
                Debug.Log("Equipped item: " + equippedItem.itemName);
                UpdateInventoryUI();
            }
        }
    }

    public void AddItem(ItemData newItem)
    {
        items.Add(newItem);
        Debug.Log("Picked up: " + newItem.itemName);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        inventoryText.text = "";
        for (int i = 0; i < items.Count; i++)
        {
            string selectedMarker = (i == selectedIndex) ? " > " : "   ";
            inventoryText.text += selectedMarker + items[i].itemName + "\n";
        }

        if (equippedItemText != null)
        {
            equippedItemText.text = equippedItem ? "Equipped: " + equippedItem.itemName : "Equipped: None";
        }
    }
}
