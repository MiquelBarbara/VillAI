using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//
/// <summary>
/// A UI button representing a single <see cref="ItemSlot"/>. 
/// Shows the item icon, stack count, and highlights selection.
/// </summary>
public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image hightlight;

    [SerializeField]
    private TextMeshProUGUI text;

    private int myIndex;

    /// <summary>
    /// Sets the index this button represents in the parent <see cref="ItemPanel"/>.
    /// </summary>
    public void SetIndex(int index)
    {
        myIndex = index;
    }

    /// <summary>
    /// Updates this button’s icon and text to show an <see cref="ItemSlot"/> state.
    /// </summary>
    /// <param name="slot">The item slot data.</param>
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

        if (slot.item.stackable)
        {
            text.gameObject.SetActive(true);
            text.text = slot.amount.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Clears the button display so no item is shown.
    /// </summary>
    public void Clean()
    {
        icon.sprite = null;
        text.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
    }

    /// <summary>
    /// Click callback from Unity’s <see cref="IPointerClickHandler"/>. 
    /// Informs the parent <see cref="ItemPanel"/> that this slot was clicked.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
        itemPanel.OnClick(myIndex);
    }

    /// <summary>
    /// Enables or disables the highlight overlay.
    /// </summary>
    /// <param name="state">True to highlight, false otherwise.</param>
    public void Highlight(bool state)
    {
        hightlight.gameObject.SetActive(state);
    }
}
