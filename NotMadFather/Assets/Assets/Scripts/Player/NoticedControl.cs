using UnityEngine;
using UnityEngine.InputSystem.XR;
using System.Collections;

public class NoticedControl : MonoBehaviour
{
    public Eye[] eyes;
    public int eyesRemaining;
    private int nextEyeIndex = 0;

    void Awake()
    {
        eyesRemaining = eyes.Length;
    }

    public void DoNotice()
    {
        if (eyesRemaining > 0)
        {
            eyes[nextEyeIndex].Open();
            eyesRemaining -= 1;
            nextEyeIndex += 1;

            SpriteRenderer player = GameObject.Find("Player").GetComponent<SpriteRenderer>();

            // visual ouchie
            if (ColorUtility.TryParseHtmlString("#F88237", out Color colour))
            {
                player.color = colour;
                StartCoroutine(ResetSprite(player));
            }
            else
            {
                Debug.LogWarning("Invalid hex color: 9451FF");
            }
        }
    }

    private IEnumerator ResetSprite(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(3f);
        spriteRenderer.color = Color.white;
    }

    public void Reset()
    {
        foreach (Eye eye in eyes)
        {
            eye.Close();
        }

        eyesRemaining = eyes.Length;
        nextEyeIndex = 0;
    }
}
