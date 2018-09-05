using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class IISConfigurationETWMonitor : BaseETWMonitor
    {
        private ETWMonitorOptions _options = new ETWMonitorOptions();

        public void InitOptions()
        {
            _options.SessionName = "IISConfigurationETWMonitorSession";
            _options.ProviderGuid = new Guid("DC0B8E51-4863-407A-BC3C-1B479B2978AC");
            _options.ProviderName = "Microsoft-Windows-IIS-Configuration";
            _options.Flags = 0xe000000000000003;
            _options.Level = TraceEventLevel.Verbose;
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
    }
}
