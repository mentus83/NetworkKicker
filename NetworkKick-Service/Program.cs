using System.ServiceProcess;

namespace NetworkKick_Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new NetWorkKickerService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
