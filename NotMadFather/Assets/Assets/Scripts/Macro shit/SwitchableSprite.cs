using UnityEngine;

public class SwitchableSprite : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite regularSprite;
    [SerializeField] private Sprite spookySprite;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite(bool medication)
    {
        spriteRenderer.sprite = medication ? regularSprite : spookySprite;
    }
}
