using GameplayFocused;

namespace UI
{
    using UnityEngine;

    public class ItemFactory : MonoBehaviour
    {
        // Assign this prefab in the Inspector. It should have the CollectableItem component.
        public GameObject collectableItemPrefab;

        public GameObject SpawnItem(Item data, Vector3 spawnPosition)
        {
            GameObject spawned = Instantiate(collectableItemPrefab, spawnPosition, Quaternion.identity);
            CollectableItem collectable = spawned.GetComponent<CollectableItem>();
            if (collectable != null)
            {
                collectable.itemData = data;
            }
            return spawned;
        }
    }

}