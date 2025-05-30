using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IdleCapability", menuName = "GOAP/Capabilities/Idle")]
    public class IdleCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new IdleCapabilityConfig();
        }
    }
}