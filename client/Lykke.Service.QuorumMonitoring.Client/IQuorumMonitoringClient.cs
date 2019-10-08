using JetBrains.Annotations;

namespace Lykke.Service.QuorumMonitoring.Client
{
    /// <summary>
    /// QuorumMonitoring client interface.
    /// </summary>
    [PublicAPI]
    public interface IQuorumMonitoringClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IQuorumMonitoringApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IQuorumMonitoringApi Api { get; }
    }
}
