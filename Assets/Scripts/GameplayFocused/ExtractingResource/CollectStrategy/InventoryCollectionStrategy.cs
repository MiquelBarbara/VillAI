using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.CollectStrategy
{
    /// <summary>
    /// A resource collection strategy that adds the collected resource directly to the character's inventory.
    /// </summary>
    public class InventoryCollectionStrategy : IResourceCollectionStrategy
    {
        /// <summary>
        /// Collects the resource by adding it to the interactor's inventory.
        /// </summary>
        /// <param name="interactor">The character collecting the resource.</param>
        /// <param name="resourceItem">The item representing the resource to collect.</param>
        /// <param name="dropPosition">This parameter is ignored in this strategy.</param>
        public void Collect(Character interactor, Item resourceItem, Vector3 dropPosition)
        {
            interactor.AddItem(resourceItem);
        }
    }
}