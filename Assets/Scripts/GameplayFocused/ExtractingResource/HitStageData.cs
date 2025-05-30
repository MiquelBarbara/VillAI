using UnityEngine;

namespace GameplayFocused.ExtractingResource
{
    /// <summary>
    /// Represents the data for a hit stage of a resource, including the sprite to display and the number of hits required.
    /// </summary>
    [System.Serializable]
    public class HitStageData
    {
        /// <summary>
        /// The sprite to display at this hit stage.
        /// </summary>
        public Sprite sprite;

        /// <summary>
        /// The number of hits required to reach this stage.
        /// </summary>
        public int hitsRequired;
    }
}