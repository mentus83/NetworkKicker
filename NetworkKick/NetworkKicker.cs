﻿using System;
using System.Linq;
using System.Management;
using System.Threading;

namespace NetworkKick
{
    public sealed class NetworkKicker
    {
        private readonly string _connectionName;

        public NetworkKicker(string connectionName)
        {
            _connectionName = connectionName;
        }

        public void Run()
        {
            var netConnection = GetNetConnection(_connectionName);

            var netStatus = ushort.Parse(netConnection["NetConnectionStatus"].ToString());

            if (netStatus == 2) return;

            var netStatusConfirmed = ConfirmNetStatus(netStatus);

            if (netStatusConfirmed) KickNetwork(netConnection);
        }

        private bool ConfirmNetStatus(ushort netStatus)
        {
            Thread.Sleep(5000);

            var newConnection = GetNetConnection(_connectionName);

            var updatedStatus = ushort.Parse(newConnection["NetConnectionStatus"].ToString());

            return netStatus == updatedStatus;
        }

        private static ManagementObject GetNetConnection(string connectionName)
        {
            var wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            var searcher = new ManagementObjectSearcher(wmiQuery);

            var netConnection = searcher.Get().Cast<ManagementObject>()
                .FirstOrDefault(item => string.Equals((string)item["NetConnectionId"], connectionName,
                StringComparison.CurrentCultureIgnoreCase));

            if (netConnection == null) throw new Exception($"Unable to find connection name [{connectionName}]");

            return netConnection;
        }

        private void KickNetwork(ManagementObject item)
        {
            try
            {
                item.InvokeMethod("Disable", null);
                Thread.Sleep(1000);
                item.InvokeMethod("Enable", null);
                OnLogContentReady(this, new LogEventArgs($"The following connection has been kicked: {_connectionName}"));
            }
            catch (Exception ex)
            {
                var nl = Environment.NewLine;
                var errorContent = $"Message:{nl}{ex.Message}{nl}Source:{nl}{ex.Source}{nl}" +
                                   $"Data:{nl}{ex.Data}{nl}Stack:{nl}{ex.StackTrace}{nl}";
                OnLogContentReady(this, new LogEventArgs($"Error occured!{nl}{errorContent}"));
            }
        }

        private void OnLogContentReady(NetworkKicker networkKicker, LogEventArgs logEventArgs)
        {
            LogContentReady?.Invoke(networkKicker, logEventArgs);
        }

        public event EventHandler<LogEventArgs> LogContentReady;
    }
}