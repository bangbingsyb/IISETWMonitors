using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISETWMonitors
{
    public delegate int ParseEvent(in TraceEvent data);

    public class ETWMonitorOptions
    {
        public string SessionName { get; set; }

        public string ProviderName { get; set; }

        public Guid ProviderGuid { get; set; }

        public ulong Flags { get; set; }

        public TraceEventLevel Level { get; set; }

        public TextWriter Writer { get; set; } = Console.Out;
    }


}
