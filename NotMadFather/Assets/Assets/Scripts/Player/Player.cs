using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    private SpriteRenderer spriteRenderer;
    private NoticedControl noticedControl;

    private Vector2 startingPosition;

    private bool isAlive = true;

    void Awake()
    {
        startingPosition = transform.position;
        controller = gameObject.GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        noticedControl = gameObject.GetComponent<NoticedControl>();
    }

    public void BeNoticed()
    {
        noticedControl.DoNotice();

        if (noticedControl.eyesRemaining <= 0) // reset/die
        {
            isAlive = false;
            spriteRenderer.enabled = false;
            controller.enabled = false;
            // empty inventory?
            // probably other stuff here too
        }
    }

    public void Respawn()
    {
        isAlive = true;
        controller.enabled = true;
        transform.position = startingPosition;
        noticedControl.Reset();
        spriteRenderer.enabled = true;
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

}
