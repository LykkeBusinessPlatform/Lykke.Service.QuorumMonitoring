using Lykke.HttpClientGenerator;

namespace Lykke.Service.QuorumMonitoring.Client
{
    /// <summary>
    /// QuorumMonitoring API aggregating interface.
    /// </summary>
    public class QuorumMonitoringClient : IQuorumMonitoringClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to QuorumMonitoring Api.</summary>
        public IQuorumMonitoringApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public QuorumMonitoringClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IQuorumMonitoringApi>();
        }
    }
}
