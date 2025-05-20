
using System.Linq;
using UnityEngine;
using System.Collections;

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
        DoctorHasLeft
    }
    private FirstSceneGameState state = FirstSceneGameState.FirstCutscene;

    private Player player;
    private AudioSource audioSource;
    private PlayerController playerController;
    private NPC doctor;

    [SerializeField] private Bed bed;
    [SerializeField] private WaypointTask taskOne;

    [SerializeField] private AudioClip alarm;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        doctor = GameObject.Find("Doctor").GetComponent<NPC>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        Vector3 fixedCameraPos = new Vector3(8.45f, -2.5f, -10.0f);
        Manager.Instance.SetCutscene(true, fixedCameraPos);

        doctor.GetComponent<WaypointController>().enabled = false;
        doctor.Speak();
        bed = GameObject.Find("Bed").GetComponent<Bed>();
        bed.gameObject.GetComponent<DialogueItem>().enabled = false;
    }

    void Update()
    {
        CheckState();

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
    }

    public void CheckState()
    {
        //  first cutscene -> task one
        if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.FirstCutscene)
        {
            state = FirstSceneGameState.TaskOne;
            doctor.GetComponent<CircleCollider2D>().radius = 0.5f;
            StartCoroutine(ActivateBed());

            string[] newDialogue = {
            "\"I understand you're having trouble with your memory.\"",
            "\"As a reminder, you must complete your mobily test by those ENVIROMENT THING.\"",
            "\"You must also compelte your reflex test by the COMPUTER?\"",
            "\"Come see me when you've completed them.\""};
            doctor.UpdateDialogue(newDialogue);
            Manager.Instance.SetCutscene(false, player.transform.position);

        }
        // player finished tasks, talk to dr
        else if (state == FirstSceneGameState.TaskOne && taskOne.isComplete)
        {
            bed.gameObject.GetComponent<DialogueItem>().enabled = false; // keep accidently getting bed dialogue
            state = FirstSceneGameState.TaskOneFinished;
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
            doctor.GetComponent<WaypointController>().enabled = true;
        }
        // dr has left the room, resume control to the player
        else if (state == FirstSceneGameState.SeccondCutsceneDrRun)
        {

            float distanceToWaypoint = Vector2.Distance(doctor.transform.position, doctor.GetComponent<WaypointController>().waypoints[0].position);
            if (distanceToWaypoint < 1)
            {
                GameObject.Find("Manager").GetComponent<AudioSource>().volume = 0.75f;
                audioSource.Stop();

                doctor.gameObject.SetActive(false);
                state = FirstSceneGameState.DoctorHasLeft;
                Manager.Instance.SetCutscene(false, player.transform.position);
                bed.gameObject.GetComponent<DialogueItem>().enabled = true;
            }
        }

    }

    private IEnumerator ActivateBed() // deactivate bed for a little after the cutscene finishes so its dialogue doesnt show immidaiytrrluy
    {
        yield return new WaitForSeconds(3f);
        bed.gameObject.GetComponent<DialogueItem>().enabled = true;
    }

}
