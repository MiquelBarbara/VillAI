using System;
using LLM.Templates.Stories;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Represents a single item in the game (e.g., a potion, weapon, etc.),
/// including its display icon, stackability, and pricing info.
/// </summary>
[Serializable]
[CreateAssetMenu(menuName = "Data/Item/Item")]
[Story("[itemName] is an item whose store price is [storePrice] and resell price is [resellPrice].")]
public class Item : ScriptableObject
{
    /// <summary>
    /// Display name of the item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// Sprite icon used in the UI.
    /// </summary>
    [JsonIgnore]
    public Sprite icon;

    /// <summary>
    /// True if multiple copies of this item can be stacked in a single slot.
    /// </summary>
    public bool stackable;

    /// <summary>
    /// The store purchase price for this item.
    /// </summary>
    public int storePrice;

    /// <summary>
    /// The price at which players can sell it back (resell).
    /// </summary>
    public int resellPrice;
    
    /// <summary>
    /// If true, the UI may highlight this item in some special fashion.
    /// </summary>
    [JsonIgnore]
    public bool iconHighlight;
}