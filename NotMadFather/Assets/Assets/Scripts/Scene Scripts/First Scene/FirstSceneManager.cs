// finn: update doctor text to like "well done"

using UnityEngine;

public class FirstSceneManager : MonoBehaviour
{
    public enum GameState
    {
        FirstCutscene,
        TaskOne,
        TaskOneFinished
    }
    public GameState state = GameState.FirstCutscene;

    public Player player;
    public PlayerController playerController;
    public NPC doctor;

    [SerializeField] private GameObject hintUI;
    [SerializeField] private WaypointTask taskOne;

    void Start()
    {
        doctor = GameObject.Find("Doctor").GetComponent<NPC>();
        doctor.currentDialogueMode = NPC.DialogueMode.ManagerController; // on start the doctor should talk to the player

        player = GameObject.Find("Player").GetComponent<Player>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        SetCutscene(true);
    }

    void Update()
    {
        CheckState();

        if (state == GameState.FirstCutscene)
        {
            if (doctor.DialogueIndex == 0) // show hint for first piece of dialogue
            {
                UIHint hintUIObj = hintUI.GetComponent<UIHint>();
                hintUIObj.ShowHint(true, this.gameObject);
            }
        }
        else if (state == GameState.TaskOne)
        {
            SetCutscene(false);
        }
        else if (state == GameState.TaskOneFinished)
        {
           // SetCutscene(true); // movve to new script for second cutscene??
        }
    }

    public void CheckState()
    {

        // starts in cutscene

        // if the player pressed e, move the dialogue along until the 2nd last line
        // the last line in dialogue[] should be the repeatable line the doctor says
        // if the player interacts with them after the cutscene
        if (doctor.DialogueIndex == doctor.Dialogue.Length - 1 && state == GameState.FirstCutscene)
        {
            state = GameState.TaskOne;
        }
        else if (state == GameState.TaskOne && taskOne.isComplete)// waypoint task correct
        {
            Debug.Log("Task One finished!");
            state = GameState.TaskOneFinished;
        }
        else // First cutscene
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                doctor.Speak();
            }
        }

    }

    public void SetCutscene(bool set)
    {
        if (set)
        {
            playerController.enabled = false;
        }
        else
        {
            doctor.currentDialogueMode = NPC.DialogueMode.PlayerInput;
            playerController.enabled = true;
        }
    }
}
