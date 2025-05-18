// for some reason you need to set the enemy's sprite renderer in the inspector... i dont know why

using UnityEngine;

public class Dangerous : SwitchableSprite
{
    private Collider2D col;
    private Rigidbody2D rb;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().BeNoticed();
        }
    }
}
