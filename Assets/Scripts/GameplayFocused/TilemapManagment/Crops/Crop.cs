using LLM.Templates.Stories;
using Newtonsoft.Json;
using UnityEngine;

namespace GameplayFocused.TilemapManagment.Crops
{
    /// <summary>
    /// Represents a crop item that can be grown in the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Crop")]
    [Story("A crop called [itemName] that sells for [resellPrice] and is bought for [storePrice] and need [timeToGrow] minutes to grow and produces [count] when harvested.")]
    public class Crop : Item
    {
        /// <summary>
        /// The time in seconds required for the crop to fully grow.
        /// </summary>
        [JsonIgnore]
        public float timeToGrow = 10f;

        /// <summary>
        /// The number of crop items produced when harvested.
        /// </summary>
        [JsonIgnore]
        public int count = 1;

        /// <summary>
        /// An array of growth stage data defining how the crop appears at each stage.
        /// </summary>
        [JsonIgnore]
        public CropGrowData[] cropGrowData;
    }
    
    /// <summary>
    /// Contains data for a specific growth stage of a crop.
    /// </summary>
    [System.Serializable]
    public class CropGrowData
    {
        /// <summary>
        /// The sprite representing the crop at this growth stage.
        /// </summary>
        [JsonIgnore]
        public Sprite sprite;

        /// <summary>
        /// The time in seconds required to reach this growth stage.
        /// </summary>
        public int growthStageTime;
    }
}