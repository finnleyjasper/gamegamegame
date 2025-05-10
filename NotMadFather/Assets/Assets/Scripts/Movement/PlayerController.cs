/*
*   Takes in keyboard input for movement
*/

using UnityEngine;

public class PlayerController : TopDownController
{
    void Update()
    {
        if (!gameObject.GetComponent<PlayerInventory>().isOpen) // turn off movement controls if inv is open
        {
            // Input
            Vector2 movementForce = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            base.CalculateMovement(movementForce);
        }
    }
}
