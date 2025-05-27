
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;

public class SecondSceneManager : MonoBehaviour
{
    private enum SecondSceneGameState
    {
        FirstCutscene,
        TaskOne,
        TaskOneFinished,
        TaskOneFinishedDoctorSpeaking,
        WaitingForSoup,
        SoupPlaced,
        DoctorLeaving,
        DoctorHasLeft,
        PlayerSpeakingSoup,
        FreeRoamBeforeTimer,
        FreeRoamAfterTimer,
        FreeRoamBeforeChange,
        FreeRoam
    }
    [SerializeField] private SecondSceneGameState state = SecondSceneGameState.FirstCutscene;

    private Player player;
    [SerializeField] private NPC playerDialogueControl;
    private AudioSource audioSource;
    private NPC doctor;

    [Header("Items")]
    [SerializeField] private PickupItem key;
    [SerializeField] private Door door;
    [SerializeField] private PickupItem soup;
    [SerializeField] private DialogueItem tray;

    [Header("Tasks")]
    [SerializeField] private WaypointTask taskOne;
    [SerializeField] private QTEInteractableItem taskTwo;

    [SerializeField] private GameObject panel;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        doctor = GameObject.Find("Doctor").GetComponent<NPC>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerDialogueControl = GameObject.Find("PlayerDialogueControl").GetComponent<NPC>();

        Vector3 vec = new Vector3(player.transform.position.x, player.transform.position.y, -1);
        Manager.Instance.SetCutscene(true, vec);

        doctor.GetComponent<WaypointController>().enabled = false;
        doctor.Speak();

        player.GetComponent<PlayerInventory>().AddItem(key.itemData);
        door.enabled = false;
        playerDialogueControl.gameObject.SetActive(false);

        panel.gameObject.SetActive(false);
        soup.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckState();

        // show initial presss E dialogue hint
        if (state == SecondSceneGameState.FirstCutscene)
        {
            if (DialogueManager.Instance.CurrentIndex == 0)
            {
                UIHint.Instance.ShowHint(true, doctor.gameObject);
            }
            else
            {
                UIHint.Instance.ShowHint(false, doctor.gameObject);
            }
        }

        // turn off QTE
        if (state == SecondSceneGameState.TaskOne && taskTwo.successful)
        {
            taskTwo.enabled = false;
        }

        // if player died
        if (!player.IsAlive)
        {
            panel.gameObject.SetActive(true);
            Invoke("RemovePanel", 3f);
        }
    }

    public void CheckState()
    {
        //  first cutscene -> task one
        if (!DialogueManager.Instance.IsDialogueActive() && state == SecondSceneGameState.FirstCutscene)
        {
            state = SecondSceneGameState.TaskOne;
            doctor.GetComponent<CircleCollider2D>().radius = 4f;

            string[] newDialogue = {
            "\"Yes, you are having trouble with your memory.\"",
            "\"Please complete your mobily and reflex tests.\"",
            "\"Come see me when you've completed them.\""};
            doctor.UpdateDialogue(newDialogue);
            Manager.Instance.SetCutscene(false, player.transform.position);

        }
        // player finished tasks, talk to dr
        else if (state == SecondSceneGameState.TaskOne && taskOne.isComplete && taskTwo.successful)
        {
            state = SecondSceneGameState.TaskOneFinished;
            string[] newDialogue = {
            "\"Thank you for completing your tests. You seem to be doing well.\"",
            "\"I will now provide you with your morning meal.\"",};
            doctor.UpdateDialogue(newDialogue);
        }
        // player must go speak to dr
        else if (state == SecondSceneGameState.TaskOneFinished)
        {
            if (doctor.RecentlyFinished)
            {
                state = SecondSceneGameState.TaskOneFinishedDoctorSpeaking;
            }
        }
        else if (state == SecondSceneGameState.TaskOneFinishedDoctorSpeaking)
        {
            Vector3 fixedCameraPos = GameObject.Find("Main Camera").transform.position;
            Manager.Instance.SetCutscene(true, fixedCameraPos);
            StartCoroutine(DelayStateChange(SecondSceneGameState.SoupPlaced, 1f));
            state = SecondSceneGameState.WaitingForSoup;
        }
        else if (state == SecondSceneGameState.SoupPlaced && !soup.gameObject.activeSelf)
        {
            soup.gameObject.SetActive(true);
            tray.gameObject.tag = "Untagged";
            Collider2D[] cols = tray.gameObject.GetComponents<Collider2D>();
            foreach (Collider2D col in cols)
            {
                col.enabled = false;
            }
            tray.enabled = false;

            string[] newDialogue = {
                "\"Be sure to finish it.\"",
                "\"I will return at your next meal time.\""
            };
            doctor.UpdateDialogue(newDialogue);
            doctor.Speak();
        }
        else if (state == SecondSceneGameState.SoupPlaced && !DialogueManager.Instance.IsDialogueActive() && doctor.RecentlyFinished)
        {
            door.gameObject.SetActive(false);
            doctor.GetComponent<WaypointController>().enabled = true;
            state = SecondSceneGameState.DoctorLeaving;
        }
        // doctor on their way out
        else if (state == SecondSceneGameState.DoctorLeaving)
        {
            float distanceToWaypoint = Vector2.Distance(doctor.transform.position, doctor.GetComponent<WaypointController>().waypoints[0].position);
            if (distanceToWaypoint < 1) // give control back to the player after dr actually left
            {
                door.gameObject.SetActive(true);
                doctor.gameObject.SetActive(false);
                state = SecondSceneGameState.DoctorHasLeft;
                door.GetComponent<DialogueItem>().enabled = false;
                door.enabled = true;
                door.tag = "Interaction";
                Manager.Instance.SetCutscene(false, player.transform.position);
            }
        }
        // player picked up soup, show dialogue
        else if (state == SecondSceneGameState.DoctorHasLeft && player.GetComponent<PlayerInventory>().ContainsItem(soup.itemData))
        {
            Vector3 fixedCameraPos = GameObject.Find("Main Camera").transform.position;
            Manager.Instance.SetCutscene(true, fixedCameraPos);
            Invoke("PlayerSpeak", 0.41f);
            state = SecondSceneGameState.PlayerSpeakingSoup;
        }
        else if (state == SecondSceneGameState.PlayerSpeakingSoup && !playerDialogueControl.gameObject.activeSelf)
        {
            Manager.Instance.SetCutscene(false, player.transform.position);
            state = SecondSceneGameState.FreeRoamBeforeTimer;
        }
        // player finished speaking, start countdown to change
        else if (state == SecondSceneGameState.FreeRoamBeforeTimer)
        {
            StartCoroutine(DelayStateChange(SecondSceneGameState.FreeRoamBeforeChange, 2f));
            state = SecondSceneGameState.FreeRoamAfterTimer;
        }
        else if (state == SecondSceneGameState.FreeRoamBeforeChange)
        {
            Vector3 fixedCameraPos = GameObject.Find("Main Camera").transform.position;
            Manager.Instance.SetCutscene(true, fixedCameraPos);
            Manager.Instance.MedicationState(false);
            Debug.Log("MEDICATION STATE: " + Manager.Instance.medication);
            string[] newD = { "\"Oh no...\"", "\"It's happening again.\"", "\"I have to know what this is.\"" };
            playerDialogueControl.UpdateDialogue(newD);
            PlayerSpeak();
            state = SecondSceneGameState.FreeRoam;
        }
        else if (state == SecondSceneGameState.FreeRoam && playerDialogueControl.RecentlyFinished)
        {
            Manager.Instance.SetCutscene(false, player.transform.position);
        }
    }

    private void PlayerSpeak()
    {
        playerDialogueControl.gameObject.SetActive(true);
        playerDialogueControl.Speak();
        playerDialogueControl.gameObject.SetActive(false);
    }

    private IEnumerator DelayDoor()
    {
        yield return new WaitForSeconds(0.4f);
        door.gameObject.SetActive(false);
    }

    private IEnumerator DelayStateChange(SecondSceneGameState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        state = newState;
    }

    private void RemovePanel()
    {
        panel.gameObject.SetActive(false);
    }

}
