using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;
using EntitiesRelated.Animation.Commands;


namespace GameplayFocused.TilemapManagment.Strategies
{
    /// <summary>
    /// Defines a strategy for executing a tile action on a farming tile, including the execution logic and related animation data.
    /// </summary>
    public interface ITileActionStrategy
    {
        /// <summary>
        /// Executes the tile action for the specified tile.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile.</param>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="character">The character performing the action.</param>
        void Execute(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data, Character character);

        /// <summary>
        /// Returns the animation action and any related event data specific to this tile action.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> and an array of <see cref="AnimationEventData"/>.
        /// </returns>
        AnimationCommand GetAnimation();
    }
}