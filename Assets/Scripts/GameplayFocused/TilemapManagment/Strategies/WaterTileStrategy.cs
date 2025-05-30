using System;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using EntitiesRelated.Animation.Commands;


namespace GameplayFocused.TilemapManagment.Strategies
{
    /// <summary>
    /// Implements a tile action strategy for watering a farming tile.
    /// </summary>
    public class WaterTileStrategy : ITileActionStrategy
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _wateredTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterTileStrategy"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="wateredTile">The tile to set when a tile is watered.</param>
        public WaterTileStrategy(Tilemap tilemap, TileBase wateredTile)
        {
            this._tilemap = tilemap;
            this._wateredTile = wateredTile;
        }

        /// <summary>
        /// Executes the watering action on the specified tile.
        /// Updates the tile's state to watered and sets the corresponding watered tile.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile to water.</param>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="character">The character performing the watering action.</param>
        public void Execute(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data, Character character)
        {
            data.state = FarmingTilemapManager.TileState.Watered;
            _tilemap.SetTile(gridPos, _wateredTile);
            Debug.Log("Tile watered at: " + gridPos);
        }

        /// <summary>
        /// Returns the animation action and associated event data for watering.
        /// </summary>
        /// <returns>
        /// A tuple containing the animation action (<see cref="AnimationAction.watering"/>) and an empty array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetAnimation()
        {
            return new WateringAnimationCommand();
        }
    }
}