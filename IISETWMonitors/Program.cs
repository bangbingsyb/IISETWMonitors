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
                IISLoggingETWMonitor.Run();
            }
            else
            {
                switch (args[0])
                {
                    case "-r":
                        IISRequestETWMonitor.Run();
                        break;
                    case "-l":
                        IISLoggingETWMonitor.Run();
                        break;
                }
            }
        }
    }
}
