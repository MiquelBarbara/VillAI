using EntitiesRelated.Core;
using GameplayFocused.TilemapManagment;
using GOAP.Scripts;
using GOAP.Scripts.Configuration.Capabilities;

namespace Game.GOAP.Scripts.Configuration.Implementations
{


    public class FarmingCapabilityConfig : ICapabilityConfig
    {
        public void Configure(GoapAgent agent)
        {
            var builder = new CapabilityBuilder("FarmingCapability");
            var character = agent.GetComponent<Character>();
            
            agent.beliefFactory.AddBelief("NearFarmTile", () => false);
            
            agent.beliefFactory.AddBelief("FieldNeedsCultivation", () => false);
            
            agent.beliefFactory.AddBelief("TileReadyToHarvest", () => false);
            
            /*agent.beliefFactory.AddBeliefWithPrediction("HaveHarvestedCrop", 
                () => false, 
                "Do I have harvested crops?");*/


            builder.AddAction(() =>
                new AgentAction.Builder("CultivateField")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["NearFarmTile"])
                    .AddPrecondition(agent.beliefs["FieldNeedsCultivation"])
                    .AddEffect(null)
                    .WithStrategy(null)
                    .Build()
            );
            
            builder.AddAction(() =>
                new AgentAction.Builder("HarvestCrop")
                    .WithCost(1)
                    .AddPrecondition(agent.beliefs["NearFarmTile"])
                    .AddPrecondition(agent.beliefs["TileReadyToHarvest"])
                    .AddEffect(agent.beliefs["HaveHarvestedCrop"])
                    .WithStrategy(null)
                    .Build()
            );

            // Goal: Obtain Harvested Crop
            builder.AddGoal(() =>
                new AgentGoal.Builder("AcquireCrops")
                    .WithDescription("Ensure the agent has harvested crops")
                    .WithPriority(4)
                    .WithDesiredEffect(agent.beliefs["HaveHarvestedCrop"])
                    .Build()
            );

            builder.Build().Configure(agent);
        }
    }
}