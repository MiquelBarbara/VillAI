using EntitiesRelated.Core;
using NPCs;
using UnityEngine;

namespace Systems.TradingSystem
{
    /// <summary>
    ///     Interface defining a trade action (Buy or Sell) to be executed in a trade session.
    /// </summary>
    public interface ITradeAction
    {
        /// <summary>
        ///     Executes the trade action (either buying or selling) between a client and a trader.
        /// </summary>
        /// <param name="client">The character who is either buying or selling (typically the player).</param>
        /// <param name="trader">The NPC acting as the trader in the transaction.</param>
        /// <param name="itemName">The name of the item being traded.</param>
        /// <param name="amount">The quantity of the item to be traded.</param>
        void Execute(Character client, Npc trader, Item item, int amount);
    }

    /// <summary>
    ///     Represents a buy action in a trade, where the client buys items from the trader.
    /// </summary>
    public class BuyAction : ITradeAction
    {
        /// <summary>
        ///     Executes the buy action, where the client purchases a specified amount of an item from the trader.
        /// </summary>
        /// <param name="client">The character (player) buying the item.</param>
        /// <param name="trader">The NPC selling the item.</param>
        /// <param name="itemName">The name of the item being bought.</param>
        /// <param name="amount">The quantity of the item being bought.</param>
        public void Execute(Character client, Npc trader, Item item, int amount)
        {
            // Remove the item from the trader's inventory
            trader.RemoveItem(item, amount);

            // Add the item to the client's inventory
            client.AddItem(item, amount);

            // The client pays the trader based on the item price and quantity
            client.EarnMoney(client.SpendMoney(item.storePrice * amount));

        }
    }

    /// <summary>
    ///     Represents a sell action in a trade, where the client sells items to the trader.
    /// </summary>
    public class SellAction : ITradeAction
    {
        /// <summary>
        ///     Executes the sell action, where the client sells a specified amount of an item to the trader.
        /// </summary>
        /// <param name="client">The character (player) selling the item.</param>
        /// <param name="trader">The NPC buying the item.</param>
        /// <param name="itemName">The name of the item being sold.</param>
        /// <param name="amount">The quantity of the item being sold.</param>
        public void Execute(Character client, Npc trader, Item item, int amount)
        {
            // Remove the item from the client's inventory
            client.RemoveItem(item, amount);

            // Add the item to the trader's inventory
            trader.AddItem(item, amount);

            // The trader pays the client based on the item's resale price and quantity
            trader.EarnMoney(trader.SpendMoney(item.resellPrice * amount));
            
        }
    }
}