using System;
using LLM.Templates.Stories;
using Newtonsoft.Json;

//
/// <summary>
/// Represents a single slot in an inventory/container, 
/// storing a reference to an <see cref="Item"/> and how many of it are present.
/// </summary>
[Serializable]
[Story("[amount] of [item].")]
public class ItemSlot
{
    /// <summary>
    /// Reference to the item in this slot; null if empty.
    /// </summary>
    public Item item;

    /// <summary>
    /// Number of items in this slot; relevant if <see cref="Item.stackable"/> is true.
    /// </summary>
    public int amount;

    /// <summary>
    /// Copies the data (item + amount) from another slot.
    /// </summary>
    /// <param name="slot">The slot to copy from.</param>
    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        amount = slot.amount;
    }

    /// <summary>
    /// Sets this slot to hold the specified item and amount.
    /// </summary>
    public void Set(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    /// <summary>
    /// Empties this slot completely, removing its item reference and count.
    /// </summary>
    public void Clear()
    {
        item = null;
        amount = 0;
    }
}