using GameplayFocused.TilemapManagment.Strategies;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment.ActionHandler
{
    /// <summary>
    /// Handles the tile action for watering by delegating to the <see cref="WaterTileStrategy"/> if applicable.
    /// </summary>
    public class WaterTileHandler : TileActionHandler
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _wateredTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterTileHandler"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="wateredTile">The tile to set when a tile is watered.</param>
        public WaterTileHandler(Tilemap tilemap, TileBase wateredTile)
        {
            this._tilemap = tilemap;
            this._wateredTile = wateredTile;
        }

        /// <summary>
        /// Attempts to handle the watering action.
        /// If the tile is in the plowed state, returns a new instance of <see cref="WaterTileStrategy"/>;
        /// otherwise, delegates handling to the next handler in the chain.
        /// </summary>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="selectedItem">The item currently selected from the toolbar.</param>
        /// <returns>An <see cref="ITileActionStrategy"/> for watering if applicable; otherwise, the result from the next handler.</returns>
        public override ITileActionStrategy Handle(FarmingTilemapManager.FarmingTileData data, Item selectedItem)
        {
            return data.state == FarmingTilemapManager.TileState.Plowed ? new WaterTileStrategy(_tilemap, _wateredTile) : NextHandler?.Handle(data, selectedItem);
        }
    }
}