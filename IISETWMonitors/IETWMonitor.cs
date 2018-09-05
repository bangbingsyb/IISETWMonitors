using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISETWMonitors
{
    interface IETWMonitor
    {
        void Configure(ETWMonitorOptions options, ParseEvent parseEventMethod = null);

        int Run();

        int ParseEvent(in TraceEvent data);
    }
}
