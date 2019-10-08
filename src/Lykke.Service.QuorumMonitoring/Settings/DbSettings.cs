using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.QuorumMonitoring.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
