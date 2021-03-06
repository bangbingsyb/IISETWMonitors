﻿using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class IISRequestTracingETWMonitor : BaseETWMonitor
    {
        private ETWMonitorOptions _options = new ETWMonitorOptions();

        public void InitOptions()
        {
            _options.SessionName = "IISRequestTracingETWMonitorSession";
            _options.ProviderGuid = new Guid("3A2A4E84-4C21-4981-AE10-3FDA0D9B0F83");
            _options.ProviderName = "IIS: WWW Server";
            _options.Flags = 0xFFFFFFFE;
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
