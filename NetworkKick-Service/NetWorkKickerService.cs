using NetworkKick;
using NetworkKick_Service.Properties;
using System;
using System.ServiceProcess;
using System.Threading;

namespace NetworkKick_Service
{
    public partial class NetWorkKickerService : ServiceBase
    {
        private Thread _worker;
        private readonly AutoResetEvent _stopRequest = new AutoResetEvent(false);

        public NetWorkKickerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _worker = new Thread(Run);
            _worker.Start();
        }

        protected override void OnStop()
        {
            _stopRequest.Set();
            _worker.Join();
        }

        private static void OnLogReady(object sender, LogEventArgs e)
        {
            Log.Write(e.Content);
        }

        private void Run()
        {
            if (_stopRequest.WaitOne(10000)) return;
            try
            {
                var frequency = Settings.Default.KickFrequency * 1000;
                var connectionName = Settings.Default.ConnectionName;
                var remoteSite = Settings.Default.RemoteSite;
                var kickLength = Settings.Default.KickLength;

                while (true)
                {
                    var netKicker = new NetworkKicker(connectionName)
                    {
                        KickLength = kickLength,
                        RemoteSite = remoteSite
                    };
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
    }
}