using System;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace GameplayFocused.TilemapManagment.Strategies
{
    /// <summary>
    /// Implements a tile action strategy for harvesting crops from a farming tile.
    /// </summary>
    public class HarvestTileStrategy : ITileActionStrategy
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _plowedTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="HarvestTileStrategy"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="plowedTile">The tile to set after harvesting (typically a plowed tile).</param>
        public HarvestTileStrategy(Tilemap tilemap, TileBase plowedTile)
        {
            this._tilemap = tilemap;
            this._plowedTile = plowedTile;
        }

        /// <summary>
        /// Executes the harvest action on the specified tile.
        /// Removes the crop from the tile, adds it to the character's inventory, and resets the tile state.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile to harvest.</param>
        /// <param name="data">The farming tile data associated with the tile.</param>
        /// <param name="character">The character performing the harvest.</param>
        public void Execute(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data, Character character)
        {
            if (data.cropTile == null) return;
            Debug.Log("Harvested crop at: " + gridPos);
            character.AddItem(data.cropTile.crop, data.cropTile.crop.count);
            data.cropTile = null;
            if (data.cropSpriteObject != null)
            {
                Object.Destroy(data.cropSpriteObject);
                data.cropSpriteObject = null;
            }
            data.state = FarmingTilemapManager.TileState.Plowed;
            _tilemap.SetTile(gridPos, _plowedTile);
        }

        /// <summary>
        /// Returns the animation action and associated event data for harvesting.
        /// </summary>
        /// <returns>
        /// A tuple containing the animation action (<see cref="AnimationAction.doing"/>) and an empty array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetAnimation()
        {
            return new DoingAnimationCommand();
        }
    }
}
