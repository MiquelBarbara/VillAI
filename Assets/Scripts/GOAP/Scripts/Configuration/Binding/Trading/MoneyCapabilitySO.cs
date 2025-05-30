using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MoneyCapability", menuName = "GOAP/Capabilities/Money")]
    public class MoneyCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new MoneyCapabilityConfig();
        }
    }
}