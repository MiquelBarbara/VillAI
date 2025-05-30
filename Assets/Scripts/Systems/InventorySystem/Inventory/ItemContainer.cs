using System.Collections.Generic;
using System.Linq;
using LLM.Templates.Stories;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

//
/// <summary>
/// An inventory-like container that stores multiple <see cref="ItemSlot"/> entries.
/// </summary>
[CreateAssetMenu(menuName = "DataContainers/Item Container")]
[AllowAsParameter]
[Story("The inventory contains the following items: [slots].")]
public class ItemContainer : ScriptableSave
{

    [SerializeField] private int currency;
    /// <summary>
    /// The list of item slots, each slot can hold a stack of one item type.
    /// </summary>
    public List<ItemSlot> slots;

    /// <summary>
    /// Flag that indicates the UI should refresh. 
    /// Managed externally (e.g. an <see cref="ItemPanel"/>).
    /// </summary>
    public bool needUpdate;

    /// <summary>
    /// Initializes the container with a fixed slot count (here 18).
    /// </summary>
    internal void Initialize()
    {
        slots = new List<ItemSlot>();
        for(int i = 0; i < 18; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    /// <summary>
    /// Adds an item (or stack) to the container, stacking if possible.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> to add.</param>
    /// <param name="count">Number of copies to add (default 1).</param>
    public void Add(Item item, int count = 1)
    {
        needUpdate = true;
        if(item.stackable)
        {
            // For stackable items, try to find an existing slot
            AddStackableSlot(item, count);
        }
        else
        {
            // For non-stackable items, just fill an empty slot
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if(itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
    }

    private void AddStackableSlot(Item item, int count)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == item);
        if(itemSlot != null)
        {
            itemSlot.amount += count;
        }
        else
        {
            // If not found, place in an empty slot
            itemSlot = slots.Find(x => x.item == null);
            if (itemSlot == null) return;
            itemSlot.item = item;
            itemSlot.amount = count;
        }
    }

    /// <summary>
    /// Removes one or more of the specified item from the container.
    /// </summary>
    /// <param name="itemToRemove">The <see cref="Item"/> to remove.</param>
    /// <param name="count">How many items to remove (default 1).</param>
    public void Remove(Item itemToRemove, int count = 1)
    {
        needUpdate = true;
        if (itemToRemove.stackable)
        {
            DecreseStackSlot(itemToRemove, count);
        }
        else
        {
            // For non-stackables, remove one copy from as many slots as needed
            while(count > 0)
            {
                count -= 1;
                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
                if(itemSlot == null) { return; }
                itemSlot.Clear();
            }
        }
    }

    private void DecreseStackSlot(Item itemToRemove, int count)
    {
        // Decrement the stack
        ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
        if (itemSlot == null) { return; }
        itemSlot.amount -= count;
          
        if (itemSlot.amount <= 0)
        {
            itemSlot.Clear();
        }
    }

    /// <summary>
    /// Checks if the container has at least as many items as specified by <paramref name="checkingItem"/>.
    /// </summary>
    /// <param name="checkingItem">The item slot (type + amount) to verify.</param>
    /// <returns>True if enough items are found, false otherwise.</returns>
    internal bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);
        if(itemSlot == null) { return false; }
        if(checkingItem.item.stackable)
        {
            return itemSlot.amount >= checkingItem.amount;
        }
        return true;
    }

    /// <summary>
    /// Returns the total count of <paramref name="item"/> in the container, or 0 if none.
    /// </summary>
    public int GetItemAmount(Item item)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == item);
        if(itemSlot == null) { return 0; }
        return itemSlot.amount;
    }

    /// <summary>
    /// Checks if the container holds at least one copy of the given <paramref name="item"/>.
    /// </summary>
    public bool HasItem(Item item)
    {
        return slots.Find(x => x.item == item) != null;
    }
    
    public List<ItemSlot> GetSlots()
    {
        return slots;
    }
    
    /// <summary>
    /// Checks if all slots are occupied (i.e. no slot is null).
    /// </summary>
    /// <returns>True if the container has no empty slot, false otherwise.</returns>
    internal bool IsFull()
    {
        return slots.All(t => t.item != null);
    }
    
    public void AddCurrency(int amount)
    {
        currency += amount;
        needUpdate = true;
    }
    
    public void RemoveCurrency(int amount)
    {
        currency -= amount;
        needUpdate = true;
    }
    
    public void SetCurrency(int amount)
    {
        currency = amount;
        needUpdate = true;
    }
    
    public int GetCurrency()
    {
        return currency;
    }
    
    public Dictionary<string, int> GetAmountByItem()
    {
        Dictionary<string, int> amountByItem = new Dictionary<string, int>();
        foreach (var itemSlot in slots.Where(itemSlot => itemSlot.item != null))
        {
            if (amountByItem.ContainsKey(itemSlot.item.name))
            {
                amountByItem[itemSlot.item.name] += itemSlot.amount;
            }
            else
            {
                amountByItem[itemSlot.item.name] = itemSlot.amount;
            }
        }
        return amountByItem;
    }
    
}
