using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//
/// <summary>
/// Handles dragging items around the UI and dropping them into the game world.
/// Works with the new Unity Input System.
/// </summary>
public class ItemDragAndDropController : MonoBehaviour
{
    /// <summary>
    /// Slot containing the currently dragged item (if any).
    /// </summary>
    public ItemSlot itemSlot;

    [SerializeField]
    private GameObject dragItemIcon;
    private RectTransform iconTransform;
    private Image dragIconImage;
    
    // Unity Input System actions
    private InputAction clickAction;
    private InputAction pointerPositionAction;

    private void Awake()
    {
        // Bind the left mouse button and pointer position
        clickAction = new InputAction("click", binding: "<Mouse>/leftButton");
        pointerPositionAction = new InputAction("pointerPosition", binding: "<Pointer>/position");
    }

    private void OnEnable()
    {
        clickAction.Enable();
        pointerPositionAction.Enable();
        clickAction.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClickPerformed;
        clickAction.Disable();
        pointerPositionAction.Disable();
    }

    private void Start()
    {
        // Initialize itemSlot if not assigned
        itemSlot ??= new ItemSlot();
        iconTransform = dragItemIcon.GetComponent<RectTransform>();
        dragIconImage = dragItemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        // If an item is being dragged, move the icon to follow the pointer
        if (!dragItemIcon.activeInHierarchy) return;
        Vector2 pointerPos = pointerPositionAction.ReadValue<Vector2>();
        iconTransform.position = pointerPos;
    }

    /// <summary>
    /// Invoked when the left mouse button is clicked.
    /// If currently dragging an item icon in the UI and we click in world space, 
    /// we spawn the item in the world and clear the slot.
    /// </summary>
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (!dragItemIcon.activeInHierarchy) return;
        itemSlot.Clear();
        dragItemIcon.SetActive(false);
    }

    /// <summary>
    /// Removes a certain count of items from this slot, e.g. when consumed or used.
    /// </summary>
    /// <param name="count">The number of items to remove. Defaults to 1.</param>
    internal void RemoveItem(int count = 1)
    {
        if (itemSlot == null) { return; }
        if (itemSlot.item.stackable)
        {
            itemSlot.amount -= count;
            if (itemSlot.amount <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            itemSlot.Clear();
        }
        UpdateIcon();
    }

    /// <summary>
    /// Checks if the slot contains at least <paramref name="count"/> of the given <paramref name="item"/>.
    /// </summary>
    public bool Check(Item item, int count = 1)
    {
        if (itemSlot == null) { return false; }
        if (item.stackable)
        {
            return itemSlot.item == item && itemSlot.amount >= count;
        }
        return itemSlot.item == item;
    }

    /// <summary>
    /// Called when the user clicks on a different item slot (e.g. from the UI), 
    /// transferring that slot's data into this drag slot or merging stacks.
    /// </summary>
    /// <param name="otherSlot">The slot from which to copy items.</param>
    internal void OnClick(ItemSlot otherSlot)
    {
        // If this drag slot is empty, just copy from the other slot
        if (this.itemSlot.item == null)
        {
            this.itemSlot.Copy(otherSlot);
            otherSlot.Clear();
        }
        else
        {
            // If they are the same item, merge
            if (otherSlot.item == this.itemSlot.item)
            {
                otherSlot.amount += this.itemSlot.amount;
                this.itemSlot.Clear();
            }
            else
            {
                // Otherwise swap
                Item item = otherSlot.item;
                int count = otherSlot.amount;
                otherSlot.Copy(this.itemSlot);
                this.itemSlot.Set(item, count);
            }
        }
        UpdateIcon();
    }

    /// <summary>
    /// Updates the drag icon to match the current item slot or hides it if empty.
    /// </summary>
    private void UpdateIcon()
    {
        if (itemSlot.item == null)
        {
            dragItemIcon.SetActive(false);
        }
        else
        {
            dragItemIcon.SetActive(true);
            dragIconImage.sprite = itemSlot.item.icon;
        }
    }
}
