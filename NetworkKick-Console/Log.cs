using System;
using System.IO;

namespace NetworkKick_Console
{
    internal static class Log
    {
        private static readonly string ErrorLogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Error_Log.txt");
        private static readonly string LogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt");

        public static void Write(Exception ex)
        {
            var nl = Environment.NewLine;
            var errorContent = $"Message:{nl}{ex.Message}{nl}Source:{nl}{ex.Source}{nl}" +
                               $"Data:{nl}{ex.Data}{nl}Stack:{nl}{ex.StackTrace}{nl}";

            File.WriteAllText(ErrorLogPath, errorContent);
        }

        internal static void Write(string content)
        {
            var nl = Environment.NewLine;
            File.WriteAllText(LogPath, content);
        }
    }
}