using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ShipComponent : MonoBehaviour
{
    public GameObject Core;
    public int Mass;

    public bool CanSpriteRotate;
    public Sprite SpriteForward, SpriteSideways;
    public Direction SpriteDireciton;

    private SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        UpdateSprite();
    }


    protected void OnDestroy()
    {
        Core.GetComponent<Rigidbody2D>().mass -= Mass;
    }

    // Update the sprite.
    public void UpdateSprite()
    {
        // If rotation is possible, rotate sprite if necessary.
        if (CanSpriteRotate)
        {
            switch (SpriteDireciton)
            {
                case Direction.Forward:
                    spriteRenderer.sprite = SpriteForward;
                    break;
                case Direction.Sideway:
                    spriteRenderer.sprite = SpriteSideways;
                    break;
            }
        }
    }

    // Direction used for instance for selecting correct sprite.
    public enum Direction
    {
        Forward, Sideway
    }
}
