
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Threading;

public class FirstSceneManager : MonoBehaviour
{
    private enum FirstSceneGameState
    {
        FirstCutscene,
        TaskOne,
        TaskOneFinished,
        TaskOneFinishedDoctorSpeaking,
        SecondCutsceneDialogue,
        SeccondCutsceneDrRun,
        DoctorHasLeft,
        EnterWithdrawl,
        InWithdrawl
    }
    private FirstSceneGameState state = FirstSceneGameState.FirstCutscene;

    private Player player;
    [SerializeField] private NPC playerDialogueControl;
    private AudioSource audioSource;
    private NPC doctor;

    [Header("Items")]
    [SerializeField] private Bed bed;
    [SerializeField] private PickupItem key;
    [SerializeField] private InteractableItem door;

    [Header("Tasks")]
    [SerializeField] private WaypointTask taskOne;
    [SerializeField] private QTEInteractableItem taskTwo;


    [SerializeField] private AudioClip alarm;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        doctor = GameObject.Find("Doctor").GetComponent<NPC>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerDialogueControl = GameObject.Find("PlayerDialogueControl").GetComponent<NPC>();

        Vector3 fixedCameraPos = new Vector3(8.45f, -2.5f, -10.0f);
        Manager.Instance.SetCutscene(true, fixedCameraPos);

        bed.GetComponent<DialogueItem>().enabled = false;
        doctor.GetComponent<WaypointController>().enabled = false;
        doctor.Speak();

        key.gameObject.SetActive(false);
        door.enabled = false;
        playerDialogueControl.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckState();

        // show initial presss E dialogue hint
        if (state == FirstSceneGameState.FirstCutscene)
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
        if (state == FirstSceneGameState.TaskOne && taskTwo.successful)
        {
            taskTwo.gameObject.SetActive(false);
        }
    }

    public void CheckState()
    {
        //  first cutscene -> task one
        if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.FirstCutscene)
        {
            bed.GetComponent<DialogueItem>().enabled = true;
            state = FirstSceneGameState.TaskOne;
            doctor.GetComponent<CircleCollider2D>().radius = 0.5f;

            string[] newDialogue = {
            "\"I understand you're having trouble with your memory.\"",
            "\"As a reminder, you must complete your mobily test by those ENVIROMENT THING.\"",
            "\"You must also compelte your reflex test by the COMPUTER?\"",
            "\"Come see me when you've completed them.\""};
            doctor.UpdateDialogue(newDialogue);
            Manager.Instance.SetCutscene(false, player.transform.position);

        }
        // player finished tasks, talk to dr
        else if (state == FirstSceneGameState.TaskOne && taskOne.isComplete && taskTwo.successful)
        {
            bed.gameObject.GetComponent<DialogueItem>().enabled = false; // keep accidently getting bed dialogue
            state = FirstSceneGameState.TaskOneFinished;
            // qte was for somereason broken just accept this jankness
            string[] newDialogue = {
            "\"Thank you for completing your tests. You seem to be doing well.\"",
            "\"It is now time for you medici-\"",};
            doctor.UpdateDialogue(newDialogue);
        }
        // player must go speak to dr
        else if (state == FirstSceneGameState.TaskOneFinished)
        {
            if (doctor.RecentlyFinished)
            {
                state = FirstSceneGameState.TaskOneFinishedDoctorSpeaking;
            }
        }
        // alarm starts, dr dialogue
        else if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.TaskOneFinishedDoctorSpeaking)
        {
            state = FirstSceneGameState.SecondCutsceneDialogue;
            Vector3 fixedCameraPos = GameObject.Find("Main Camera").transform.position;
            Manager.Instance.SetCutscene(true, fixedCameraPos);

            GameObject.Find("Manager").GetComponent<AudioSource>().volume -= 0.5f;
            audioSource.clip = alarm;
            audioSource.loop = true;
            audioSource.Play();

            string[] newDialogue = {
            "\"What is that?\"",
            "\"Please hold on! I will be right back...\"",};
            doctor.UpdateDialogue(newDialogue);
            doctor.Speak();
        }
        // dialogue finish, dr runs out
        else if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.SecondCutsceneDialogue)
        {
            state = FirstSceneGameState.SeccondCutsceneDrRun;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            doctor.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            doctor.GetComponent<WaypointController>().enabled = true;
        }
        // dr has left the room, resume control to the player
        else if (state == FirstSceneGameState.SeccondCutsceneDrRun)
        {

            float distanceToWaypoint = Vector2.Distance(doctor.transform.position, doctor.GetComponent<WaypointController>().waypoints[0].position);
            if (distanceToWaypoint < 1) // give control back to the player after dr actually left
            {
                GameObject.Find("Manager").GetComponent<AudioSource>().volume = 0.75f;
                audioSource.Stop();

                doctor.gameObject.SetActive(false);
                state = FirstSceneGameState.DoctorHasLeft;
                Manager.Instance.SetCutscene(false, player.transform.position);
                bed.gameObject.GetComponent<DialogueItem>().enabled = true;

                // start a timer for player to experience withdrawl
                StartCoroutine(DelayedWithdrawlChange());
            }
        }
        // player is free to roam in withdrawl after player dialogue
        else if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.EnterWithdrawl)
        {
            // door turns from dialogue item into an interactable item
            door.GetComponent<DialogueItem>().enabled = false;
            door.enabled = true;
            door.tag = "Interaction";

            Manager.Instance.SetCutscene(false, player.transform.position);
            key.gameObject.SetActive(true);

            state = FirstSceneGameState.InWithdrawl;
        }
        else if (state == FirstSceneGameState.InWithdrawl && door.interactedWith)
        {

        }

    }

    private void PlayerSpeak()
    {
        playerDialogueControl.gameObject.SetActive(true);
        playerDialogueControl.Speak();
        playerDialogueControl.gameObject.SetActive(false);
    }

    private IEnumerator DelayedWithdrawlChange()
    {
        yield return new WaitForSeconds(15f);
        state = FirstSceneGameState.EnterWithdrawl;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // player is sliding?
        Vector3 fixedCameraPos = GameObject.Find("Main Camera").transform.position;
        Manager.Instance.SetCutscene(true, fixedCameraPos);
        Manager.Instance.MedicationState(false);
        PlayerSpeak();
    }

}
