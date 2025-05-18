
using System.Linq;
using UnityEngine;

public class FirstSceneManager : MonoBehaviour
{
    private enum FirstSceneGameState
    {
        FirstCutscene,
        TaskOne,
        TaskOneFinished
    }
    private FirstSceneGameState state = FirstSceneGameState.FirstCutscene;

    private Player player;
    private PlayerController playerController;
    private NPC doctor;

    [SerializeField] private WaypointTask taskOne;

    void Start()
    {
        doctor = GameObject.Find("Doctor").GetComponent<NPC>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        Vector3 fixedCameraPos = new Vector3(8f, -3f, -10.0f);
        Manager.Instance.SetCutscene(true, fixedCameraPos);
    }

    void Update()
    {
        CheckState();

        if (state == FirstSceneGameState.FirstCutscene) // dr dialogue
        {
            // show hint for first piece of dialogue
            if (DialogueManager.Instance.CurrentIndex == 0)
            {
                UIHint.Instance.ShowHint(true, doctor.gameObject);
            }
            else
            {
                UIHint.Instance.ShowHint(false, doctor.gameObject);
            }
        }
        else if (state == FirstSceneGameState.TaskOne) // let player go do tests
        {

        }
        else if (state == FirstSceneGameState.TaskOneFinished)
        {
            // SetCutscene(true); // movve to new script for second cutscene??
        }
    }

    public void CheckState()
    {
        if (DialogueManager.Instance.CurrentIndex == 5 && state == FirstSceneGameState.FirstCutscene) // dr finished speaking -> do tasks
        {
            Debug.Log("NEW SCENE TIME");
            state = FirstSceneGameState.TaskOne;
            string[] newDialogue = {
                "Dummy text",
                "\"I understand you're having trouble with your memory.\"",
                "\"As a reminder, you must complete your mobily test by those ENVIROMENT THING.\"",
                "\"You must also compelte your reflex test by the COMPUTER?\"",
                "\"Come see me when you've completed them.\""};
            doctor.UpdateDialogue(newDialogue);
            Manager.Instance.SetCutscene(false, player.transform.position);
        }
        else if (state == FirstSceneGameState.TaskOne && taskOne.isComplete) // task completed -> dr to give medication
        {
            Debug.Log("Task One finished!");
            state = FirstSceneGameState.TaskOneFinished;
        }
        else // First cutscene
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                doctor.Speak();
            }
        }

    }
}

