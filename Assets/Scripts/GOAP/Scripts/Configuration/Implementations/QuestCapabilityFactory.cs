using GOAP.Scripts;

using GOAP.Scripts.Configuration.Capabilities;


public class QuestCapabilityConfig : ICapabilityConfig
{
    public void Configure(GoapAgent agent)
    {
        // 1. Create a builder for the "QuestCapability"
        var builder = new CapabilityBuilder("QuestCapability");
        var blackboard = agent.BlackboardController.GetBlackboard();

        // -------------------------------------
        // 2. Register any blackboard keys you need
        // For example, store the "current quest" or "quest giver" or "quest board" position.
        // If your game has a central QuestBoard or quest giver, you might have a transform or an object reference:
        var questBoardKey = blackboard.GetOrRegisterKey("QuestBoardPosition"); 
        // We'll assume something in your game sets the blackboard "QuestBoardPosition" to the position/Transform of a quest board.

        var questActiveKey = blackboard.GetOrRegisterKey("HasActiveQuest");
        blackboard.SetValue(questActiveKey, false);

        var questCompleteKey = blackboard.GetOrRegisterKey("IsQuestComplete");
        blackboard.SetValue(questCompleteKey, false);

        // -------------------------------------
        // 3. Define beliefs
        //
        //    For instance, “AgentHasActiveQuest” checks the blackboard “HasActiveQuest”
        //    “QuestIsComplete” checks “IsQuestComplete”
        //    “QuestAvailable” might check if we can see a quest or if the quest board has open quests, etc.

        agent.beliefFactory.AddBelief("AgentHasActiveQuest", () =>
        {
            // This is a simple example: a boolean in the blackboard
            if (blackboard.TryGetValue(questActiveKey, out bool hasQuest))
                return hasQuest;
            return false;
        });

        agent.beliefFactory.AddBelief("QuestIsComplete", () =>
        {
            if (blackboard.TryGetValue(questCompleteKey, out bool isComplete))
                return isComplete;
            return false;
        });

        // If you want to sense whether a quest is posted at the quest board,
        // you could write a custom Sensor or just store a bool in the blackboard
        // set by some other system. For example:
        agent.beliefFactory.AddBelief("QuestAvailableOnBoard", () =>
        {
            // For a real game, you might poll a "QuestBoard" component to see if there's a quest available
            // We'll pretend it's always true, or from blackboard
            // e.g. blackboard.TryGetValue("QuestAvailable", out bool questAvailable)
            return true;
        });

        // -------------------------------------
        // 4. Define actions 
        //    (PickUpQuest, CompleteQuest, GiveItemToQuestGiver, or whichever your quests require.)

        // (A) Move to the quest board
        builder.AddAction(() =>
            new AgentAction.Builder("GoToQuestBoard")
                .WithCost(1)
                .AddPrecondition(agent.beliefs["QuestAvailableOnBoard"]) // We only do this if a quest is available
                .AddEffect(agent.beliefs["AgentHasActiveQuest"])         // Our effect is that we end up having a quest
                .WithStrategy(new MoveStrategy(agent.gameObject, blackboard, questBoardKey))
                .Build()
        );

        // (B) PickUpQuest 
        // You could combine “GoToQuestBoard” + “PickUpQuest” or just do it in a single action. 
        // But splitting them is more flexible in GOAP planning.
        builder.AddAction(() =>
            new AgentAction.Builder("PickUpQuest")
                .WithCost(1)
                .AddPrecondition(agent.beliefs["QuestAvailableOnBoard"])
                .AddPrecondition(agent.beliefs["AgentAtQuestBoard"]) 
                // You can define an "AgentAtQuestBoard" location-based belief with .AddLocationBelief 
                .AddEffect(agent.beliefs["AgentHasActiveQuest"])
                // The actual “picking up quest” logic would go in a custom IAgentStrategy
                .WithStrategy(new PickUpQuestStrategy(agent, blackboard))
                .Build()
        );

        // (C) CompleteQuest
        // For a typical quest like “deliver items,” your action would move to the quest giver or quest board 
        // and run a strategy that checks if the agent has the items, then sets “IsQuestComplete=true”
        builder.AddAction(() =>
            new AgentAction.Builder("CompleteQuest")
                .WithCost(2)
                .AddPrecondition(agent.beliefs["AgentHasActiveQuest"])
                .AddEffect(agent.beliefs["QuestIsComplete"])
                .WithStrategy(new CompleteQuestStrategy(agent, blackboard))
                .Build()
        );

        // If you need extra steps—like “Collect item X from the world,” “Give item to NPC,” etc.—
        // define additional actions the same way.

        // -------------------------------------
        // 5. Define a goal to “Finish a quest if you have one”
        // 
        //    The desired effect: "QuestIsComplete" is true
        builder.AddGoal(() =>
            new AgentGoal.Builder("FinishQuest")
                .WithPriority(5)
                .WithDesiredEffect(agent.beliefs["QuestIsComplete"])
                .Build()
        );

        // 6. Build & apply to the agent
        builder.Build().Configure(agent);
    }
}
