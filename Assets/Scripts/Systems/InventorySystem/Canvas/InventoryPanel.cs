using GameplayFocused;

//
/// <summary>
/// Specialized <see cref="ItemPanel"/> for a standard inventory. 
/// When a slot is clicked, calls the global <see cref="ItemDragAndDropController"/> to handle the item movement.
/// </summary>
public class InventoryPanel : ItemPanel
{
    /// <summary>
    /// When a slot is clicked, pass that slot to the <see cref="ItemDragAndDropController"/> for potential drag & drop.
    /// </summary>
    /// <param name="id">Index of the clicked slot.</param>
    public override void OnClick(int id)
    {
        GameManager.Instance.dragAndDropController.OnClick(inventory.slots[id]);
        Show();
    }
    
    public void Inject(ItemContainer container)
    {
        inventory = container;
        Initialize();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}