/*
*   Made this class abstract so enemies and npcs will be able to use it with different inputs
*       (keyboard vs waypoints or whatever)
*/

using UnityEngine;

public abstract class TopDownController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private Vector2 lastMoveDir = Vector2.down;

    [Header("Idle Sprites")]
    public Sprite idleDown;
    public Sprite idleUp;
    public Sprite idleRight;

    [Header("Walking Animations")]
    public RuntimeAnimatorController walkAnimator; // Optional, used only when moving

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = null; // Ensure we start idle
    }

    protected void CalculateMovement(Vector2 movementForce)
    {
        movement = movementForce;
        // Store last movement direction
        if (movement != Vector2.zero)
        {
            lastMoveDir = movement;

            // Enable walk animations
            if (walkAnimator != null && animator.runtimeAnimatorController != walkAnimator)
                animator.runtimeAnimatorController = walkAnimator;

            // Flip sprite for left
            if (movement.x < 0)
                spriteRenderer.flipX = true;
            else if (movement.x > 0)
                spriteRenderer.flipX = false;

            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                animator.SetInteger("direction", 1); // Right or Left

                if (movement.x < 0)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
            }
            else if (movement.y > 0)
            {
                animator.SetInteger("direction", 2); // Up
                spriteRenderer.flipX = false;
            }
            else if (movement.y < 0)
            {
                animator.SetInteger("direction", 0); // Down
                spriteRenderer.flipX = false;
            }
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);

            // Remove animator to switch to static sprite
            animator.runtimeAnimatorController = null;

            // Select idle sprite based on last move direction
            if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
            {
                spriteRenderer.sprite = idleRight;
                spriteRenderer.flipX = lastMoveDir.x < 0;
            }
            else
            {
                spriteRenderer.flipX = false;
                if (lastMoveDir.y > 0)
                    spriteRenderer.sprite = idleUp;
                else
                    spriteRenderer.sprite = idleDown;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
