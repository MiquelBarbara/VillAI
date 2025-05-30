namespace GOAP.Scripts.Configuration.Capabilities
{
    /// <summary>
    /// Defines the configuration for a capability.
    /// Implementations inject capabilities into a <see cref="GoapAgent"/> by adding actions, goals, sensors, and stat configurations.
    /// </summary>
    public interface ICapabilityConfig
    {
        /// <summary>
        /// Configures the given <see cref="GoapAgent"/> with the capability.
        /// </summary>
        /// <param name="agent">The agent to configure.</param>
        void Configure(GoapAgent agent);
    }
}