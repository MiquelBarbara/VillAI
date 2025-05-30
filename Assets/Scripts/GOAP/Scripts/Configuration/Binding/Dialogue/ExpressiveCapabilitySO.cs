using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ExpressiveCapability", menuName = "GOAP/Capabilities/Expressive")]
    public class ExpressiveCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new ExpressiveCapabilityConfig();
        }
    }
}