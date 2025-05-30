using Game.GOAP.Scripts.Configuration.Implementations;
using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SocialInteractionCapability", menuName = "GOAP/Capabilities/SocialInteraction")]
    public class SocialInteractionCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new SocialInteractionCapabilityConfig();
        }
    }
}