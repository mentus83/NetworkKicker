using NetworkKick;
using NetworkKick_Console;
using NetworkKick_Service.Properties;
using System;
using System.ServiceProcess;
using System.Threading;

namespace NetworkKick_Service
{
    public partial class NetWorkKickerService : ServiceBase
    {
        public NetWorkKickerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var frequency = Settings.Default.KickFrequency * 1000;
                var connectionName = Settings.Default.ConnectionName;

                while (true)
                {
                    var netKicker = new NetworkKicker(connectionName);
                    netKicker.LogContentReady += OnLogReady;
                    netKicker.Run();
                    Thread.Sleep(frequency);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        protected override void OnStop()
        {
        }

        private static void OnLogReady(object sender, LogEventArgs e)
        {
            Log.Write(e.Content);
        }
    }
}
