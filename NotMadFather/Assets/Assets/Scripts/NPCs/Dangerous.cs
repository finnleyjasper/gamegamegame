using UnityEngine;

public class Dangerous : MonoBehaviour
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
            collision.gameObject.GetComponent<Player>().Die();
        }
    }
}
