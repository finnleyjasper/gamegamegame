

using UnityEngine;

public class WaypointTask : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    public int counter;
    public bool isComplete;
    [SerializeField] AudioClip successClip;
    private bool clipPlayed;

    void Update()
    {
        if (counter >= 3 && isComplete == false) // players needs to walk between points 3 times
        {
            isComplete = true;
        }

        if (isComplete)
        {

            if (!clipPlayed)
            {
                AudioSource audioSource = GameObject.Find("FirstSceneManager").GetComponent<AudioSource>();
                audioSource.clip = successClip;
                audioSource.loop = false;

                audioSource.Play();
                clipPlayed = true;

                foreach (GameObject waypoint in waypoints)
                {
                    string[] newDialogue = { "\"I've already finished this task\""};
                    waypoint.GetComponent<DialogueItem>().UpdateDialogue(newDialogue);
                }
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
