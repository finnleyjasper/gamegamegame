using UnityEngine;

public class SwitchableSprite : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite regularSprite;
    [SerializeField] private Sprite spookySprite;
    protected SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite(bool medication)
    {
        // TO DO: fix this
        //spriteRenderer.sprite = medication ? regularSprite : spookySprite;
    }
}
