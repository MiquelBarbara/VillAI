using System;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using EntitiesRelated.Animation.Commands;


namespace GameplayFocused.TilemapManagment.Strategies
{
    /// <summary>
    /// Implements a tile action strategy for plowing a farming tile.
    /// </summary>
    public class PlowTileStrategy : ITileActionStrategy
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _plowedTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlowTileStrategy"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="plowedTile">The tile to set when a tile is plowed.</param>
        public PlowTileStrategy(Tilemap tilemap, TileBase plowedTile)
        {
            this._tilemap = tilemap;
            this._plowedTile = plowedTile;
        }

        /// <summary>
        /// Executes the plowing action on the specified tile.
        /// Updates the tile's state and sets the corresponding plowed tile.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile to plow.</param>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="character">The character performing the plowing action.</param>
        public void Execute(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data, Character character)
        {
            data.state = FarmingTilemapManager.TileState.Plowed;
            _tilemap.SetTile(gridPos, _plowedTile);
        }

        /// <summary>
        /// Returns the animation action and associated event data for plowing.
        /// </summary>
        /// <returns>
        /// A tuple containing the animation action (<see cref="AnimationAction.dig"/>) and an empty array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetAnimation()
        {
            return new IdleAnimationCommand();
        }
    }
}