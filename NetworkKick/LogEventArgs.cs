using System;

namespace NetworkKick
{
    public class LogEventArgs : EventArgs
    {
        public string Content { get; set; }

        public LogEventArgs(string content)
        {
            Content = content;
        }
    }
}