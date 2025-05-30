using GameplayFocused.TilemapManagment.Strategies;

namespace GameplayFocused.TilemapManagment.ActionHandler
{
    /// <summary>
    /// Abstract base class for handling tile actions using a chain-of-responsibility pattern.
    /// </summary>
    public abstract class TileActionHandler
    {
        /// <summary>
        /// The next handler in the chain.
        /// </summary>
        protected TileActionHandler NextHandler;

        /// <summary>
        /// Sets the next tile action handler in the chain.
        /// </summary>
        /// <param name="next">The next handler.</param>
        public void SetNext(TileActionHandler next)
        {
            NextHandler = next;
        }

        /// <summary>
        /// Handles the tile action for the given farming tile data and selected item.
        /// </summary>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="selectedItem">The item currently selected from the toolbar.</param>
        /// <returns>
        /// An <see cref="ITileActionStrategy"/> to execute the tile action, or null if this handler cannot handle the action.
        /// </returns>
        public abstract ITileActionStrategy Handle(FarmingTilemapManager.FarmingTileData data, Item selectedItem);
    }
}