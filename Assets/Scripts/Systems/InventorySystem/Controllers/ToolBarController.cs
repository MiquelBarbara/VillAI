using System;
using System.Collections;
using System.Collections.Generic;
using GameplayFocused;
using UnityEngine;

//
/// <summary>
/// Manages a toolbar of items (e.g. a hotbar), letting the user scroll through
/// items and highlights the currently selected slot.
/// </summary>
public class ToolbarController : MonoBehaviour
{
    [SerializeField]
    private int toolbarSize = 6; // Number of slots in the toolbar

    private int selectedItem;    // Index of the currently selected slot

    /// <summary>
    /// Fired whenever the selected slot changes, passing the new index.
    /// </summary>
    public Action<int> onChange;

    [SerializeField]
    private IconHightlight iconHightlight; // UI component that highlights or displays icon

    /// <summary>
    /// Gets the currently selected <see cref="ItemSlot"/> from the global inventory.
    /// </summary>
    public ItemSlot GetItemSlot => GameManager.Instance.inventory.slots[selectedItem];

    /// <summary>
    /// Gets the <see cref="Item"/> in the selected slot, or null if none.
    /// </summary>
    public Item GetItem => GameManager.Instance.inventory.slots[selectedItem].item;

    private void Start()
    {
        // Subscribe a method to the onChange event to update UI highlights
        onChange += UpdateHightlightIcon;
        // Initialize the highlight
        UpdateHightlightIcon(selectedItem);
    }

    /// <summary>
    /// Sets the currently selected item slot by index.
    /// </summary>
    /// <param name="id">Slot index to select.</param>
    internal void Set(int id)
    {
        selectedItem = id;
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        switch (delta)
        {
            case 0:
                return;
            // Scrolling forward => move backward in the toolbar
            case > 0:
                selectedItem -= 1;
                selectedItem = (selectedItem < 0 ? toolbarSize - 1 : selectedItem);
                break;
            default:
                // Scrolling backward => move forward in the toolbar
                selectedItem += 1;
                selectedItem = (selectedItem >= toolbarSize ? 0 : selectedItem);
                break;
        }

        onChange?.Invoke(selectedItem);
    }

    /// <summary>
    /// Updates the highlight icon to display the currently selected item's icon, 
    /// or hides the icon if no item is present.
    /// </summary>
    /// <param name="id">The newly selected slot index (unused in the method body).</param>
    public void UpdateHightlightIcon(int id = 0)
    {
        Item item = GetItem;
        if (item == null)
        {
            iconHightlight.Show = false;
            return;
        }

        iconHightlight.Show = item.iconHighlight; // Show/hide highlight
        if (item.iconHighlight)
        {
            iconHightlight.Set(item.icon);
        }
    }
}
