using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    private RectTransform _cursorRect;

    private void Start()
    {
        _cursorRect = GetComponent<RectTransform>();
        Cursor.visible = false; // Hide default cursor
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        _cursorRect.position = mousePos; // Move UI cursor to mouse position
    }
}