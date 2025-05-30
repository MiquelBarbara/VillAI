using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.TilemapManagment
{
    /// <summary>
    /// A wrapper for a farming tile that implements the <see cref="Interactable"/> interface,
    /// allowing interaction with individual tiles in the farming tilemap.
    /// </summary>
    public class FarmingTileWrapper : Interactable
    {
        /// <summary>
        /// Gets the grid position of the tile.
        /// </summary>
        private Vector3Int GridPos { get; set; }

        /// <summary>
        /// Gets the farming tile data associated with this tile.
        /// </summary>
        private FarmingTilemapManager.FarmingTileData TileData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FarmingTileWrapper"/> class.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile.</param>
        /// <param name="data">The tile data associated with the tile.</param>
        public FarmingTileWrapper(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data)
        {
            GridPos = gridPos;
            TileData = data;
        }

        /// <summary>
        /// Delegates the interaction to the FarmingTilemapManager.
        /// </summary>
        /// <param name="character">The character interacting with the tile.</param>
        public void Interact(Character character)
        {
            FarmingTilemapManager.Instance.InteractAtTile(GridPos, TileData, character);
        }

        /// <summary>
        /// Gets the transform of the farming tile, which is the transform of the FarmingTilemapManager.
        /// </summary>
        /// <returns>The transform component of the FarmingTilemapManager.</returns>
        public Transform GetTransform()
        {
            return FarmingTilemapManager.Instance.transform;
        }

        /// <summary>
        /// Gets the interaction animation for the tile based on its current data.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> and an array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetInteractionAnimation()
        {
            return FarmingTilemapManager.Instance.GetAnimationForTile(TileData);
        }
    }
}
