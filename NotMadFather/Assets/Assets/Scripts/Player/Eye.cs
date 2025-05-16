using UnityEngine;

public class Eye : MonoBehaviour
{
    public bool isOpen = false;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = close;
    }

    public void Open()
    {
        isOpen = true;
        spriteRenderer.sprite = open;
    }

    public void Close() // for reset purposes - or is there's a "heal" mechanic
    {
        isOpen = false;
        spriteRenderer.sprite = close;
    }
}
