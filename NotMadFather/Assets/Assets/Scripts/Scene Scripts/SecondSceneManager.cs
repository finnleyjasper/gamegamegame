
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;

public class SecondSceneManager : MonoBehaviour
{
    private enum FirstSceneGameState
    {
        FirstCutscene,
        TaskOne,
        TaskOneFinished,
        TaskOneFinishedDoctorSpeaking,
        SecondCutsceneDialogue,
    }
    private FirstSceneGameState state = FirstSceneGameState.FirstCutscene;

    private Player player;
    [SerializeField] private NPC playerDialogueControl;
    private AudioSource audioSource;
    private NPC doctor;

    [Header("Items")]
    [SerializeField] private PickupItem key;
    [SerializeField] private InteractableItem door;

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

        Vector3 fixedCameraPos = new Vector3(8.45f, -2.5f, -10.0f);
        Manager.Instance.SetCutscene(true, fixedCameraPos);

        doctor.GetComponent<WaypointController>().enabled = false;
        doctor.Speak();

        player.GetComponent<PlayerInventory>().AddItem(key.itemData);
        door.enabled = false;
        playerDialogueControl.gameObject.SetActive(false);

        panel.gameObject.SetActive(false);
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
        if (!DialogueManager.Instance.IsDialogueActive() && state == FirstSceneGameState.FirstCutscene)
        {
            state = FirstSceneGameState.TaskOne;
            doctor.GetComponent<CircleCollider2D>().radius = 4f;

            string[] newDialogue = {
            "\"Yes, you are having trouble with your memory.\"",
            "\"Please complete your mobily and reflex tests.\"",
            "\"Come see me when you've completed them.\""};
            doctor.UpdateDialogue(newDialogue);
            Manager.Instance.SetCutscene(false, player.transform.position);

        }
        // player finished tasks, talk to dr
        else if (state == FirstSceneGameState.TaskOne && taskOne.isComplete && taskTwo.successful)
        {
            state = FirstSceneGameState.TaskOneFinished;
            // qte was for somereason broken just accept this jankness
            string[] newDialogue = {
            "\"Thank you for completing your tests. You seem to be doing well.\"",
            "\"I will now provide you with your morning meal.\"",};
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

    private void RemovePanel()
    {
        panel.gameObject.SetActive(false);
    }

}
