using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("Game quit");
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
