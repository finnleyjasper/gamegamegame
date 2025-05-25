using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("UI Elements")]

    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI equippedItemText;
    public SpriteRenderer equippedItemImage;

    [Header("Inventory")]
    public List<ItemData> items = new List<ItemData>();
    private int selectedIndex = -1;
    public ItemData equippedItem;
    public ItemData noneItem; // the placeholder "nothing equipped" item. dunno if this is how it should be done

    public bool isOpen = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        equippedItem = noneItem;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
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

            if (Input.GetKeyDown(KeyCode.E) && selectedIndex >= 0)
            {
                if (items[selectedIndex] == equippedItem) // unequip item if selected again
                {
                    equippedItem = noneItem;
                    Debug.Log("Unequipped item:" + items[selectedIndex].itemName);
                    ;
                }
                else  // equip the selected item
                {
                    equippedItem = items[selectedIndex];
                    Debug.Log("Equipped item: " + equippedItem.itemName);
                }
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

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            equippedItem = noneItem;
            Debug.Log("Removed: " + item.name);
            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Can't remove " + item.name + " as it is not in the inventory");
        }
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
            equippedItemImage.sprite = equippedItem ? equippedItem.icon : equippedItemImage.sprite = noneItem.icon;
        }
    }

    public bool ContainsItem(ItemData item)
    {
        return items.Contains(item);
    }
}
