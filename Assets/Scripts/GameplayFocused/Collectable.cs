using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused
{
    /// <summary>
    /// Represents an item in the world that can be collected by the player.
    /// When the player collides with the collectable, the item is added to the player's inventory.
    /// </summary>
    public class CollectableItem : MonoBehaviour
    {
        /// <summary>
        /// The item data associated with this collectable.
        /// </summary>
        public Item itemData;

        /// <summary>
        /// The number of items provided by this collectable.
        /// </summary>
        public int count = 1;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = itemData.icon;
        }
    
        /// <summary>
        /// Updates the collectable's item data and refreshes its sprite.
        /// </summary>
        /// <param name="item">The new item data.</param>
        /// <param name="count">The count of the item.</param>
        public void Set(Item item, int count)
        {
            itemData = item;
            this.count = count;
            _spriteRenderer.sprite = item.icon;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Character character))
                character.AddItem(itemData, count);
            
            Destroy(gameObject);
        }
    }
}