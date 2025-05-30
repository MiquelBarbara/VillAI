using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.CollectStrategy
{
    /// <summary>
    /// A resource collection strategy that drops the collected resource into the world as a physical object.
    /// </summary>
    public class WorldDropCollectionStrategy : IResourceCollectionStrategy
    {
        /// <summary>
        /// The prefab that will be instantiated in the world as the collected resource.
        /// </summary>
        private readonly GameObject _collectableItemPrefab;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldDropCollectionStrategy"/> class.
        /// </summary>
        /// <param name="collectableItemPrefab">The prefab to instantiate for the collected item.</param>
        public WorldDropCollectionStrategy(GameObject collectableItemPrefab)
        {
            this._collectableItemPrefab = collectableItemPrefab;
        }
    
        /// <summary>
        /// Collects the resource by instantiating the collectable item prefab at the specified drop position.
        /// Sets the sprite and item data on the prefab if possible.
        /// </summary>
        /// <param name="interactor">The character collecting the resource.</param>
        /// <param name="resourceItem">The item to be collected.</param>
        /// <param name="dropPosition">The world position where the resource should be dropped.</param>
        public void Collect(Character interactor, Item resourceItem, Vector3 dropPosition)
        {
            if (_collectableItemPrefab == null) return;
            
            if (_collectableItemPrefab.TryGetComponent(out SpriteRenderer spriteRenderer))
                spriteRenderer.sprite = resourceItem.icon;
            
            if (_collectableItemPrefab.TryGetComponent(out CollectableItem collectableItem))
                collectableItem.itemData = resourceItem;
                
            Object.Instantiate(_collectableItemPrefab, dropPosition, Quaternion.identity);
        }
    }
}
