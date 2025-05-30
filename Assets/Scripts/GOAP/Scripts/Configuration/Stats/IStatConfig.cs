using GOAP.Scripts;
using GOAP.Scripts.AgentStats;

namespace GOAP.Configuration
{
    /// <summary>
    ///     Interfaz para configurar una estadística en el StatSystem.
    /// </summary>
    public interface IStatConfig
    {
        void Configure(StatSystem statSystem);
    }
}