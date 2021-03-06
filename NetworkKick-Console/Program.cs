﻿using NetworkKick;
using NetworkKick_Console.Properties;
using System;
using System.Threading;

namespace NetworkKick_Console
{
    internal static class Program
    {
        private static void Main(string[] args)
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

        private static void OnLogReady(object sender, LogEventArgs e)
        {
            Log.Write(e.Content);
        }
    }
}