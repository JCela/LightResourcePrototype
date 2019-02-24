using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliderSize : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = _spriteRenderer.size;
    }
}
