using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISETWMonitors
{
    class Program
    {
        static void Main(string[] args)
        {
            string cmdOption = (args.Length == 0) ? "-l" : args[0];

            switch (cmdOption)
            {
                case "-l":
                    IISRequestLoggingETWMonitor reqLogMonitor = new IISRequestLoggingETWMonitor();
                    reqLogMonitor.Run();
                    break;
                case "-t":
                    IISRequestTracingETWMonitor reqTraceMonitor = new IISRequestTracingETWMonitor();
                    reqTraceMonitor.Run();
                    break;
                case "-c":
                    IISConfigurationETWMonitor configMonitor = new IISConfigurationETWMonitor();
                    configMonitor.Run();
                    break;
                default:
                    Console.WriteLine("Invalid switch!");
                    break;
            }
        }
    }
}
