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
            ClearLogIfLong(ErrorLogPath);
            var nl = Environment.NewLine;
            var errorContent = $"[{DateTime.Now:dd-MM-yyyy_HH:mm:ss}] Message:{nl}{ex.Message}{nl}Source:{nl}{ex.Source}{nl}" +
                               $"Data:{nl}{ex.Data}{nl}Stack:{nl}{ex.StackTrace}{nl}";

            File.AppendAllText(ErrorLogPath, errorContent);
        }

        internal static void Write(string content)
        {
            ClearLogIfLong(LogPath);
            var nl = Environment.NewLine;
            File.AppendAllText(LogPath, $"[{DateTime.Now:dd-MM-yyyy_HH:mm:ss}] {content}{nl}");
        }

        private static void ClearLogIfLong(string logPath)
        {
            if (File.Exists(logPath) && new FileInfo(logPath).Length > 10000) File.Delete(logPath);
        }
    }
}