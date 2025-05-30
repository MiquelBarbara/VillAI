using Game.GOAP.Scripts.Configuration.Implementations;
using GameplayFocused.ExtractingResource.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MineralGatherCapability", menuName = "GOAP/Capabilities/MineralGather")]
    public class MineralGatherCapability: CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new GatherCapabilityConfig<MineableRock>();
        }
    }
}