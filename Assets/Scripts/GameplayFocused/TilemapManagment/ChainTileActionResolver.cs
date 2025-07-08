using GameplayFocused.TilemapManagment.ActionHandler;
using GameplayFocused.TilemapManagment.Strategies;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment
{
    /// <summary>
    /// Resolves the appropriate tile action strategy using a chain-of-responsibility pattern.
    /// </summary>
    public class ChainTileActionResolver
    {
        private readonly TileActionHandler _chain;
        private readonly ToolbarController _toolbar;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainTileActionResolver"/> class.
        /// Sets up and chains the various tile action handlers.
        /// </summary>
        /// <param name="tilemap">The Tilemap containing the farming tiles.</param>
        /// <param name="plowedTile">The TileBase representing a plowed tile.</param>
        /// <param name="wateredTile">The TileBase representing a watered tile.</param>
        /// <param name="seededTile">The TileBase representing a seeded tile.</param>
        /// <param name="cropSpritePrefab">The prefab used for crop sprites.</param>
        /// <param name="toolbar">The ToolbarController that provides the currently selected item.</param>
        public ChainTileActionResolver(Tilemap tilemap, TileBase plowedTile, TileBase wateredTile, TileBase seededTile, GameObject cropSpritePrefab, ToolbarController toolbar)
        {
            this._toolbar = toolbar;
        
            // Create and chain the tile action handlers.
            var harvestHandler = new HarvestTileHandler(tilemap, plowedTile);
            var plantHandler = new PlantTileHandler(tilemap, seededTile, cropSpritePrefab, toolbar);
            var plowHandler = new PlowTileHandler(tilemap, plowedTile);
            
            plowHandler.SetNext(plantHandler);
            plantHandler.SetNext(harvestHandler);

            _chain = plowHandler;
        }

        /// <summary>
        /// Resolves and returns the appropriate tile action strategy based on the given farming tile data.
        /// </summary>
        /// <param name="data">The farming tile data for which to resolve an action strategy.</param>
        /// <returns>
        /// An instance of <see cref="ITileActionStrategy"/> if a matching strategy is found; otherwise, null.
        /// </returns>
        public ITileActionStrategy Resolve(FarmingTilemapManager.FarmingTileData data)
        {
            // Retrieve the currently selected item from the toolbar.
            Item selectedItem = _toolbar.GetItem;
            return _chain.Handle(data, selectedItem);
        }
    }
}
