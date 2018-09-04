using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class IISLoggingETWMonitor
    {
        public static readonly string SessionName = "IISLoggingETWMontitorSession";
        public static readonly Guid ProviderGuid = new Guid("7E8AD27F-B271-4EA2-A783-A47BDE29143B");
        public const string ProviderName = "Microsoft-Windows-IIS-Logging";
        public static ulong Flags = 0x8000000000000000;
        public static TraceEventLevel Level = TraceEventLevel.Informational;
        public static TextWriter Out = Console.Out;

        public static int Run()
        {
            Out.WriteLine("************************** IISLoggingETWMonitor **************************");
            Out.WriteLine("This program generates processes and displays IIS request logging ETW events");
            Out.WriteLine();

            // You have to be Admin to turn on ETW events (anyone can write ETW events).
            if (!(TraceEventSession.IsElevated() ?? false))
            {
                Out.WriteLine("To turn on ETW events you need to be Administrator, please run from an Admin process.");
                Debugger.Break();
                return -1;
            }

            Out.WriteLine("Creating a '{0}' session", SessionName);
            // Out.WriteLine("Use 'logman query -ets' to see active sessions.");
            // Out.WriteLine("Use 'logman stop {0} -ets' to manually stop orphans.", SessionName);

            // Create a TraceEventSession
            using (var session = new TraceEventSession(SessionName))
            {
                // Control C handler
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) { session.Dispose(); };

                // Hook up events. Hook up a callback for every event that 'Dynamic' knows about.
                session.Source.Dynamic.All += delegate (TraceEvent data)
                {
                    ParseEvent(in data);
                };

                // Enable my provider, you can call many of these on the same session to get events from other providers.  
                // Because this EventSource did not define any keywords, I can only turn on all events or none.  
                var restarted = session.EnableProvider(ProviderGuid, Level, Flags);
                if (restarted)
                {
                    Out.WriteLine("The session {0} was already active, it has been restarted.", SessionName);
                }

                Out.WriteLine("**** Start listening for events from the provider: {0}.", ProviderName);
                session.Source.Process();
                Out.WriteLine();
                Out.WriteLine("**** Stopping the collection of events.");
            }
            return 0;
        }

        private static int ParseEvent(in TraceEvent data)
        {
            string requestLogString = string.Empty;

            // Ignore the first field EnabledFieldsFlags -  start with index 1
            for (int i = 1; i < data.PayloadNames.Length; i++)
            {
                requestLogString += (data.PayloadNames[i] + " " + data.PayloadString(i) + " ");
            }
            Out.WriteLine(requestLogString);

            return 0;
        }
    }
}
