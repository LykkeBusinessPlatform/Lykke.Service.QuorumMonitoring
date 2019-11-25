using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Logs;
using Lykke.Service.QuorumMonitoring.Settings;
using Nethereum.JsonRpc.WebSocketClient;
using Nethereum.Web3;

namespace Lykke.Service.QuorumMonitoring.Services
{
    public class MonitoringService : IStartable
    {
        private readonly List<NodeItem> _nodes;
        private readonly ILog _log;
        private readonly TimerTrigger _timer;
        private List<(string, Web3)> _nodesConnection = new List<(string, Web3)>();

        public MonitoringService(List<NodeItem> nodes, ILogFactory log)
        {
            _nodes = nodes;
            _log = log.CreateLog("QuorumScanner");
            _timer = new TimerTrigger(this.GetType().Name, TimeSpan.FromSeconds(10), EmptyLogFactory.Instance, DoTime);
        }

        private async Task DoTime(ITimerTrigger timer, TimerTriggeredHandlerArgs args, CancellationToken cancellationtoken)
        {

            await Task.Delay(20);
            Console.WriteLine(">>>Start check");

            var list = new List<Report>();
            foreach (var node in _nodesConnection)
            {
                var report = await TestNode(node.Item1, node.Item2);
                _log.Info("Quorum node report", context: new
                {
                    QuorumReport = report
                });
            }

            foreach (var report in list)
            {
                _log.Info("Quorum node report", context: new
                {
                    QuorumReport = report
                });
            }
            Console.WriteLine("<<<Stop check");
        }

        private async Task<Report> TestNode(string name, Web3 web3)
        {
            Nethereum.Hex.HexTypes.HexBigInteger lastBlock = null;
            var success = false;

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                lastBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                success = true;
            }
            catch(Exception ex)
            {
                _log.Info("Cannot get last block from node.", context:new
                {
                    Name = name
                }, exception: ex);
            }
            sw.Stop();

            var report = new Report
            {
                Name = name,
                LastBlock = (long)(lastBlock?.Value ?? 0),
                LatencyMs = sw.ElapsedMilliseconds,
                Latency = sw.Elapsed,
                IsSuccess = success
            };

            return report;
        }

        private Web3 InternalCreateInstanceApi(string connString)
        {
            Web3 web3;
            if (connString.StartsWith("ws"))
            {
                var wsClient = new WebSocketClient(connString);
                web3 = new Web3(wsClient);
            }
            else
            {
                web3 = new Web3(connString);
            }

            return web3;
        }

        public void Start()
        {
            foreach (var node in _nodes)
            {
                _nodesConnection.Add((node.Name, InternalCreateInstanceApi(node.ConnectionString)));
            }
            _timer.Start();
        }
    }

    public class Report
    {
        public string Name { get; set; }
        public long LatencyMs { get; set; }
        public TimeSpan Latency { get; set; }
        public long LastBlock { get; set; }
        public bool IsSuccess { get; set; }
    }
}
