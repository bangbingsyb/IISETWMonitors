using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class IISRequestLoggingETWMonitor : BaseETWMonitor
    {
        private ETWMonitorOptions _options = new ETWMonitorOptions();

        public void InitOptions()
        {
            _options.SessionName = "IISRequestLoggingETWMonitorSession";
            _options.ProviderGuid = new Guid("7E8AD27F-B271-4EA2-A783-A47BDE29143B");
            _options.ProviderName = "Microsoft-Windows-IIS-Logging";
            _options.Flags = 0x8000000000000000;
            _options.Level = TraceEventLevel.Informational;
            _options.Writer = Console.Out;
        }

        public new int Run()
        {
            if (IsConfigured == false)
            {
                InitOptions();
                Configure(_options, ParseEvent);
            }

            base.Run();

            return 0;
        }

        public new int ParseEvent(in TraceEvent data)
        {
            string requestLogString = string.Empty;

            // Ignore the first field EnabledFieldsFlags -  start with index 1
            for (int i = 1; i < data.PayloadNames.Length; i++)
            {
                requestLogString += (data.PayloadNames[i] + " " + data.PayloadString(i) + " ");
            }
            Writer.WriteLine(requestLogString);

            return 0;
        }
    }
}
