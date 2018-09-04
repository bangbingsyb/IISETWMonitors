using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class IISRequestETWMonitor
    {
        public static readonly string SessionName = "IISRequestETWMontitorSession";
        public static readonly Guid ProviderGuid = new Guid("3A2A4E84-4C21-4981-AE10-3FDA0D9B0F83");
        public const string ProviderName = "IIS: WWW Server";
        public static ulong Flags = 0xFFFFFFFE;
        public static TraceEventLevel Level = TraceEventLevel.Verbose;
        public static TextWriter Out = Console.Out;

        public static int Run()
        {
            Out.WriteLine("************************** IISRequestETWMonitor **************************");
            Out.WriteLine("This program generates processes and displays IIS request-based ETW events");
            Out.WriteLine("using the ETW REAL TIME pipeline. (thus no files are created)");
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
                // Here we install the Control C handler.   It is OK if Dispose is called more than once.  
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

                Out.WriteLine("**** Start listening for events from the provider: {0}", ProviderName);
                session.Source.Process();
                Out.WriteLine();
                Out.WriteLine("**** Stopping the collection of events.");
            }
            return 0;
        }

        private static int ParseEvent(in TraceEvent data)
        {
            Out.WriteLine("IIS request tracing event: {0}", data.OpcodeName);

            for (int i = 0; i < data.PayloadNames.Length; i++)
            {
                Out.WriteLine("Name: {0} | Value: {1}", data.PayloadNames[i], data.PayloadString(i));
            }

            return 0;
        }
    }
}
