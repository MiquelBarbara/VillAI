using Game.GOAP.Scripts.Configuration.Implementations;
using GameplayFocused.ExtractingResource;
using GameplayFocused.ExtractingResource.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AnimalCapability", menuName = "GOAP/Capabilities/AnimalCapability")]
    public class AnimalGatherCapability: CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new GatherCapabilityConfig<AnimalResource>();
        }
    }
}