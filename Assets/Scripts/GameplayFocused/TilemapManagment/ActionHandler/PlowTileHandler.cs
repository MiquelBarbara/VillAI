using GameplayFocused.TilemapManagment.Strategies;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment.ActionHandler
{
    /// <summary>
    /// Handles the tile action for plowing a tile using a chain-of-responsibility pattern.
    /// </summary>
    public class PlowTileHandler : TileActionHandler
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _plowedTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlowTileHandler"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="plowedTile">The tile to set when a tile is plowed.</param>
        public PlowTileHandler(Tilemap tilemap, TileBase plowedTile)
        {
            this._tilemap = tilemap;
            this._plowedTile = plowedTile;
        }

        /// <summary>
        /// Attempts to handle the plowing action.
        /// If the tile is unplowed, returns a new instance of <see cref="PlowTileStrategy"/>;
        /// otherwise, delegates handling to the next handler in the chain.
        /// </summary>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="selectedItem">The item currently selected from the toolbar.</param>
        /// <returns>An <see cref="ITileActionStrategy"/> for plowing if applicable; otherwise, the result from the next handler.</returns>
        public override ITileActionStrategy Handle(FarmingTilemapManager.FarmingTileData data, Item selectedItem)
        {
            return data.state == FarmingTilemapManager.TileState.Unplowed ? new PlowTileStrategy(_tilemap, _plowedTile) : NextHandler?.Handle(data, selectedItem);
        }
    }
}