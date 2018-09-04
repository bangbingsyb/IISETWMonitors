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
            if (args.Length == 0)
            {
                IISRequestLoggingETWMonitor.Run();
            }
            else
            {
                switch (args[0])
                {
                    case "-t":
                        IISRequestTracingETWMonitor.Run();
                        break;
                    case "-l":
                        IISRequestLoggingETWMonitor.Run();
                        break;
                }
            }
        }
    }
}
