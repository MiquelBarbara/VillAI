using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace Game.GOAP.Scripts.Configuration.Implementations
{

public class CraftingCapabilityConfig : ICapabilityConfig
{
    public void Configure(GoapAgent agent)
    {
        // 1) Create builder
        var builder = new CapabilityBuilder("CraftingCapability");
        var blackboard = agent.BlackboardController.GetBlackboard();
        
        
        var itemKey = blackboard.GetOrRegisterKey("NextItemToBuy");
        
        
        var craftStationKey = blackboard.GetOrRegisterKey("CraftStationPosition");

        var hasAllIngredientsKey = blackboard.GetOrRegisterKey("HasAllIngredients");
        blackboard.SetValue(hasAllIngredientsKey, false);
        
        var hasCraftedItemKey = blackboard.GetOrRegisterKey("HasCraftedItem");
        blackboard.SetValue(hasCraftedItemKey, false);
        
        var inventory = agent.GetComponent<InventoryController>();
        if (inventory == null)
        {
            Debug.LogWarning($"{agent.name} has no IInventory. Crafting won't work.");
        }
        
        agent.beliefFactory.AddBelief("AgentHasAllIngredients", () => false);
        
        agent.beliefFactory.AddLocationBelief("AgentAtCraftStation", 1, Vector3.zero);
        
        agent.beliefFactory.AddBelief("CraftIsDone", () => blackboard.TryGetValue(hasCraftedItemKey, out bool done) && done);
        
        builder.AddAction(() =>
            new AgentAction.Builder("MoveToCraftStation")
                .WithCost(1)
                // Precondition: We actually have items to craft, or want to craft
                .AddPrecondition(agent.beliefs["AgentHasAllIngredients"])
                .AddEffect(agent.beliefs["AgentAtCraftStation"])
                .WithStrategy(new MoveStrategy(agent.gameObject, blackboard, craftStationKey))
                .Build()
        );
        
        builder.AddAction(() =>
            new AgentAction.Builder("CraftItem")
                .WithCost(2)
                .AddPrecondition(agent.beliefs["AgentHasAllIngredients"])
                .AddPrecondition(agent.beliefs["AgentAtCraftStation"]) // if needed
                .AddEffect(agent.beliefs["CraftIsDone"])               // once crafted, we assume “craft done”
                .WithStrategy(new CraftStrategy(agent, blackboard))
                .Build()
        );
        
        builder.AddGoal(() =>
            new AgentGoal.Builder("ObtainCraftedItem")
                .WithPriority(3)
                .WithDesiredEffect(agent.beliefs["CraftIsDone"])
                .Build()
        );
        
        builder.Build().Configure(agent);
    }
}
}