using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    private SpriteRenderer spriteRenderer;

    private Vector2 startingPosition;

    private bool isAlive = true;

    void Awake()
    {
        startingPosition = transform.position;
        controller = gameObject.GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Die()
    {
        isAlive = false;
        spriteRenderer.enabled = false;
        controller.enabled = false;
        // empty inventory?
        // probably other stuff here too
    }

    public void Respawn()
    {
        isAlive = true;
        controller.enabled = true;
        transform.position = startingPosition;
        spriteRenderer.enabled = true;
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

}
