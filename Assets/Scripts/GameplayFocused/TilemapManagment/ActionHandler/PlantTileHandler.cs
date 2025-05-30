using GameplayFocused.TilemapManagment.Crops;
using GameplayFocused.TilemapManagment.Strategies;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment.ActionHandler
{
    /// <summary>
    /// Handles the tile action for planting seeds on a tile using a chain-of-responsibility pattern.
    /// </summary>
    public class PlantTileHandler : TileActionHandler
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _seededTile;
        private readonly GameObject _cropSpritePrefab;
        private ToolbarController _toolbar;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlantTileHandler"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="seededTile">The tile to set when a seed is planted.</param>
        /// <param name="cropSpritePrefab">The prefab used for the crop's visual representation.</param>
        /// <param name="toolbar">The toolbar controller that provides the currently selected item.</param>
        public PlantTileHandler(Tilemap tilemap, TileBase seededTile, GameObject cropSpritePrefab, ToolbarController toolbar)
        {
            this._tilemap = tilemap;
            this._seededTile = seededTile;
            this._cropSpritePrefab = cropSpritePrefab;
            this._toolbar = toolbar;
        }

        /// <summary>
        /// Attempts to handle the planting action.
        /// If the selected item is a seed and the tile is in an appropriate state, returns a strategy for planting.
        /// </summary>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="selectedItem">The item currently selected from the toolbar.</param>
        /// <returns>
        /// An <see cref="ITileActionStrategy"/> for planting if applicable; otherwise, the result from the next handler.
        /// </returns>
        public override ITileActionStrategy Handle(FarmingTilemapManager.FarmingTileData data, Item selectedItem)
        {
            if (selectedItem is not SeedItem seed) return NextHandler?.Handle(data, selectedItem);
            return data.state switch
            {
                // If the tile is unplowed, first plow it.
                FarmingTilemapManager.TileState.Unplowed => new PlowTileStrategy(
                    _tilemap, /* fallback: using seededTile for plowed state */ _seededTile),
                // If the tile is plowed or watered, proceed to plant the seed.
                FarmingTilemapManager.TileState.Plowed or FarmingTilemapManager.TileState.Watered =>
                    new PlantTileStrategy(_tilemap, _seededTile, _cropSpritePrefab),
                _ => NextHandler?.Handle(data, selectedItem)
            };
        }
    }
}
