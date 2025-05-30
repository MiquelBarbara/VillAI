using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IconHightlight : MonoBehaviour
{
    public Vector3Int cellPosition;
    Vector3 _targetPosition;
    [SerializeField] Tilemap targetTilemap;
    SpriteRenderer _spriteRenderer;

    private bool _canSelect;
    private bool _show;
    
    public bool Show
    {
        set
        {
            _show = value;
            gameObject.SetActive(_canSelect && _show);
        }
    }
    private void Update()
    {
        _targetPosition = targetTilemap.CellToWorld(cellPosition);
        transform.position = _targetPosition + targetTilemap.cellSize/2;
    }

    internal void Set(Sprite sprite)
    {
        if(_spriteRenderer == null) { _spriteRenderer = GetComponent<SpriteRenderer>(); }
        _spriteRenderer.sprite = sprite;
    }
}