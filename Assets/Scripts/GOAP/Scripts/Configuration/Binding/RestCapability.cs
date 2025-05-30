using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RestCapability", menuName = "GOAP/Capabilities/Rest")]
    public class RestCapability : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new RestCapabilityConfig();
        }
    }
}