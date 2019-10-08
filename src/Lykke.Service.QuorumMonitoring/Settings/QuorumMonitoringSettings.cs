using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.QuorumMonitoring.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class QuorumMonitoringSettings
    {
        public DbSettings Db { get; set; }

        public List<NodeItem> Nodes { get; set; }
    }

    public class NodeItem
    {
        public string ConnectionString { get; set; }

        public string Name { get; set; }
    }
}
