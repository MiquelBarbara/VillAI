using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles crafting functionality in the game.
/// </summary>
public class Crafting : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;
    public void Craft(CraftingRecipe recipe)
    {
        if(inventory.IsFull())
        {
            return;
        }

        if (recipe.elements.Any(t => inventory.CheckItem(t) == false))
        {
            return;
        }
        
        foreach (var t in recipe.elements)
        {
            inventory.Remove(t.item, t.amount);
        }

        inventory.Add(recipe.output.item, recipe.output.amount);
    }
}
