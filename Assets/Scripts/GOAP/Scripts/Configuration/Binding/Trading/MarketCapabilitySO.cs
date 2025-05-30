using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MarketCapability", menuName = "GOAP/Capabilities/Market")]
    public class MarketCapabilitySO: CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new MarketCapabilityConfig();
        }
    }
}