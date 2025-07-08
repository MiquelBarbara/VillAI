using System;
using UnityEngine;

namespace GameplayFocused.TilemapManagment.Crops
{
    /// <summary>
    /// Represents a tile on which a crop is planted and tracks its growth status.
    /// </summary>
    [Serializable]
    public class CropTile
    {
        /// <summary>
        /// The accumulated growth time on this tile.
        /// </summary>
        public int growTimer;

        /// <summary>
        /// The current growth stage of the crop.
        /// </summary>
        public int growStage;

        /// <summary>
        /// The crop planted on this tile.
        /// </summary>
        public Crop crop;

        /// <summary>
        /// The SpriteRenderer used to display the crop.
        /// </summary>
        public SpriteRenderer renderer;

        /// <summary>
        /// The damage value applied to the crop (if any).
        /// </summary>
        public float damage;

        /// <summary>
        /// The grid position of the crop tile.
        /// </summary>
        public Vector3Int position;

        /// <summary>
        /// Gets a value indicating whether the crop has completed its growth.
        /// </summary>
        public bool Complete
        {
            get 
            {
                if (crop == null || crop.cropGrowData == null) { return false; }
                return growStage >= crop.cropGrowData.Length - 1;
            }
        }

        /// <summary>
        /// Resets the crop tile after it has been harvested.
        /// </summary>
        internal void Harvested()
        {
            growTimer = 0;
            growStage = 0;
            damage = 0;
            crop = null;
            renderer.gameObject.SetActive(false);
        }
    }
}