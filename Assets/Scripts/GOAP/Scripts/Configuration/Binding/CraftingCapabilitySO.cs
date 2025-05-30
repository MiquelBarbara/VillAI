using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CraftingCapability", menuName = "GOAP/Capabilities/Crafting")]
    public class CraftingCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new CraftingCapabilityConfig();
        }
    }
}