using System.Collections.Generic;
using TMPro;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public enum GameState
    {
        Cutscene,
        Playable
    }
    public GameState state = GameState.Playable;

    [Header("WORLD CONTROL")]
    public bool medication = true; // true if the player takes it, and turns false when they look at a memory

    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject overlay;
    [SerializeField] private TMP_FontAsset scaryFont;
    [SerializeField] private TMP_FontAsset normalFont;

    [Header("Sound")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip medicationMusic;
    [SerializeField] private AudioClip spookyMusic;

    [Header("Other stuff")]
    public Player player;

    void Awake()
    {
        medication = true;

        try
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        catch
        {
            Debug.LogWarning("Player could not be found");
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        overlay = GameObject.Find("Overlay");
        overlay.SetActive(false);

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = medicationMusic;
        audioSource.Play();

        UpdateFonts(normalFont);
    }

    void Update()
    {
        PlayerInventory playerInv = player.GetComponent<PlayerInventory>();
        //player uses medication
        if (playerInv.equippedItem.name == "Soup" && Input.GetKeyDown(KeyCode.Space) && !medication)
        {
            medication = true;
            Debug.Log("Medication state is: true");
            MedicationState(true);
        }

        // player looks at photo
        if (playerInv.equippedItem.name == "Memento" && Input.GetKeyDown(KeyCode.Space))
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

// PLAYER CONTROL ******************************************************************************

    private void Respawn()
    {
        SceneManager.LoadScene("SecondScene");
    }

// MEDICATION SWITCH UPDATES ******************************************************************************

    public void MedicationState(bool medicationState) // true and world is normal, false and switches to weird withdrawl shit
    {
        if (medicationState) // normal
        {
            medication = true;
            UpdateFonts(normalFont);
            UpdateSprites(medication);
            overlay.SetActive(false);
            audioSource.clip = medicationMusic;
            audioSource.Play();
            // plus other stuff
        }
        else // withdrawl
        {
            medication = false;
            UpdateFonts(scaryFont);
            UpdateSprites(medication);
            overlay.SetActive(true);
            audioSource.clip = spookyMusic;
            audioSource.Play();
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

        UIHint.Instance.UpdateFont(font);
        TMP_Text dialogText = gameObject.GetComponent<DialogueManager>().dialogueText;
        TMP_Text dialogNameText = gameObject.GetComponent<DialogueManager>().speakerName;
        dialogText.font = font;
        dialogNameText.font = font;
    }

    private void UpdateSprites(bool medication) // ie.. true == medication taken
    {
        SwitchableSprite[] sprites = Object.FindObjectsByType<SwitchableSprite>(FindObjectsSortMode.None);

        foreach (SwitchableSprite sprite in sprites)
        {
            sprite.UpdateSprite(medication);
        }
    }

    // STATE CONTROL ******************************************************************************

    public void SetCutscene(bool set, Vector3 cameraPosition) // if false, player position should be parsed
    {
        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
        GameObject camera = GameObject.Find("Main Camera");

        if (set)
        {
            camera.transform.SetParent(null);
            playerController.enabled = false;

        }
        else
        {
            camera.transform.SetParent(player.transform);
            playerController.enabled = true;
            cameraPosition = new Vector3(cameraPosition.x, cameraPosition.y, -10); // the z axis gets fucked otherwise
        }

        camera.transform.position = cameraPosition;
    }
}
