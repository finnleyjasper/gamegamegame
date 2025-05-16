using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("WORLD CONTROL")]
    public bool medication = true; // true if the player takes it, and turns false when they look at a memory

    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject overlay;
    [SerializeField] private TMP_FontAsset scaryFont;
    [SerializeField] private TMP_FontAsset normalFont;

    [Header("Other stuff")]
    public Player player;


    void Awake()
    {
        try
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        catch
        {
            Debug.LogWarning("Player could not be found");
        }

        overlay = GameObject.Find("Overlay");
        overlay.SetActive(false);
    }

    void Update()
    {
        PlayerInventory playerInv = player.GetComponent<PlayerInventory>();

        //player uses medication
        if (playerInv.equippedItem.name == "Medication" && Input.GetKeyDown(KeyCode.Space) && !medication)
        {
            medication = true;
            Debug.Log("Medication state is: true");
            MedicationState(true);
        }

        // player looks at photo
        if (playerInv.equippedItem.name == "Photo" && Input.GetKeyDown(KeyCode.Space))
        {
            medication = false;
            Debug.Log("Medication state is: false");
            MedicationState(false);

        }

        if (!player.IsAlive)
        {
            Invoke("Respawn", 2);
        }
    }

    private void Respawn()
    {
        player.Respawn();
        // reset enemies, items, the world etc etc
    }

    public void MedicationState(bool state) // true and world is normal, false and switches to weird withdrawl shit
    {
        if (state)
        {
            UpdateFonts(normalFont);
            overlay.SetActive(false);
            // plus other stuff
        }
        else
        {
            UpdateFonts(scaryFont);
            overlay.SetActive(true);
            // plus other stuff
        }

    }

    private void UpdateFonts(TMP_FontAsset font)
    {
        TextMeshProUGUI[] textObjects = canvas.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in textObjects)
        {
            if (text)
            {
                text.font = font;
            }

        }

    }
}
