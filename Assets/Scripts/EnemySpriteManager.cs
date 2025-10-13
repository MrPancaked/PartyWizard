using System;
using UnityEngine;

public abstract class EnemySpriteManager : MonoBehaviour
{
    [SerializeField] private protected SpriteRenderer spriteRenderer;

    protected void UpdateSprite(Vector3 direction)
    {
        if (direction.x < -0.1f) spriteRenderer.flipX = true;
        else if (direction.x > 0.1f) spriteRenderer.flipX = false;
    }
}
