using GOAP.Scripts.Configuration.Capabilities;
using UnityEngine;

namespace GOAP.Scripts.Configuration.Binding
{
    /// <summary>
    /// Base class for all capabilities using ScriptableObjects.
    /// </summary>
    public abstract class CapabilitySO : ScriptableObject
    {
        /// <summary>
        /// Returns the factory for this capability.
        /// </summary>
        public abstract ICapabilityConfig  GetConfig();
    }
}