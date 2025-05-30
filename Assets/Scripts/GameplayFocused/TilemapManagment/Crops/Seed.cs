using LLM.Templates.Stories;
using UnityEngine;

namespace GameplayFocused.TilemapManagment.Crops
{
    /// <summary>
    /// Represents a seed item used for planting crops.
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Seed")]
    [Story("This item is a seed to plant: [cropToPlant].")]
    public class SeedItem : Item
    {
        /// <summary>
        /// The crop that will be planted when using this seed.
        /// </summary>
        public Crop cropToPlant;
    }
}