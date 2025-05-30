using GOAP.Scripts.Configuration.Binding;
using GOAP.Scripts.Configuration.Capabilities;
using GOAP.Scripts.Configuration.Implementations;
using UnityEngine;

namespace GOAP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WorkCapabilitySO", menuName = "GOAP/Capabilities/WorkCapabilitySO")]
    public class WorkCapabilitySO : CapabilitySO
    {
        public override ICapabilityConfig GetConfig()
        {
            return new WorkCapabilityConfig();
        }
    }
}