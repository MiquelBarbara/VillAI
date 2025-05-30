using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
/// <summary>
/// A base panel for displaying <see cref="ItemContainer"/> slots. Manages UI <see cref="InventoryButton"/>s
/// to show item icons and amounts.
/// </summary>
public class ItemPanel : MonoBehaviour
{
    /// <summary>
    /// The <see cref="ItemContainer"/> displayed by this panel.
    /// </summary>
    public ItemContainer inventory;

    /// <summary>
    /// The list of UI buttons representing each slot in the container.
    /// </summary>
    public List<InventoryButton> buttons;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Sets slot indices and updates the UI immediately.
    /// </summary>
    protected void Initialize()
    {
        SetIndex();
        Show();
    }

    private void OnEnable()
    {
        Show();
    }

    private void LateUpdate()
    {
        // If the container signals an update is needed, refresh the UI
        if (inventory.needUpdate)
        {
            Show();
            inventory.needUpdate = false;
        }
    }

    /// <summary>
    /// Assigns each <see cref="InventoryButton"/> its corresponding slot index for click handling.
    /// </summary>
    private void SetIndex()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i);
        }
    }

    /// <summary>
    /// Updates all UI buttons to match the current state of <see cref="ItemContainer.slots"/>.
    /// </summary>
    public virtual void Show()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }
    }

    /// <summary>
    /// Invoked by a button when clicked, passing the slot index. Base does nothing.
    /// </summary>
    /// <param name="id">Index of the clicked slot.</param>
    public virtual void OnClick(int id)
    {

    }
}
