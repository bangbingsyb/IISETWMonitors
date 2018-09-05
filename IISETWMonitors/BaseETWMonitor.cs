using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IISETWMonitors
{
    class BaseETWMonitor : IETWMonitor
    {
        private string _sessionName;
        private Guid _providerGuid;
        private string _providerName;
        private ulong _flags;
        private TraceEventLevel _level;
        private TextWriter _writer;
        private bool _fConfigured = false;
        private ParseEvent _parseEventMethod;

        public TextWriter Writer
        {
            get { return _writer; }
        }

        public bool IsConfigured
        {
            get { return _fConfigured; }
        }

        public void Configure(ETWMonitorOptions options, ParseEvent parseEventMethod = null)
        {
            _sessionName = options.SessionName;
            _providerGuid = options.ProviderGuid;
            _providerName = options.ProviderName;
            _flags = options.Flags;
            _level = options.Level;
            _writer = options.Writer;
            _parseEventMethod = (parseEventMethod == null) ? ParseEvent : parseEventMethod;
            _fConfigured = true;
        }

        public int Run()
        {
            if (!(TraceEventSession.IsElevated() ?? false))
            {
                Writer.WriteLine("To turn on ETW events you need to be Administrator, please run from an Admin process.");
                Debugger.Break();
                return -1;
            }

            if (_fConfigured == false)
            {
                Writer.WriteLine("The monitor has not been configured.");
                Debugger.Break();
                return -1;
            }

            Writer.WriteLine("Creating an ETW session: {0}", _sessionName);
            Writer.WriteLine();

            // Create a TraceEventSession
            using (var session = new TraceEventSession(_sessionName))
            {
                // Control C handler
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) { session.Dispose(); };

                // Hook up a callback for every event the Dynamic parser knows about.
                session.Source.Dynamic.All += delegate (TraceEvent data)
                {
                    _parseEventMethod(in data);
                };

                var restarted = session.EnableProvider(_providerGuid, _level, _flags);
                if (restarted)
                {
                    Writer.WriteLine("The session {0} was already active, it has been restarted.", _sessionName);
                }

                Writer.WriteLine("**** Start listening for events from the provider: {0}.", _providerName);
                session.Source.Process();
                Writer.WriteLine();
                Writer.WriteLine("**** Stopping the collection of events.");
            }
            return 0;
        }

        public int ParseEvent(in TraceEvent data)
        {
            Writer.WriteLine("EventName: {0}", data.EventName);
            Writer.WriteLine("Provider: {0}", data.ProviderName);
            Writer.WriteLine("Opcode: {0}", data.OpcodeName);
            Writer.WriteLine("TimeStamp: {0}", data.TimeStamp);

            for (int i = 0; i < data.PayloadNames.Length; i++)
            {
                Writer.WriteLine("{0}: {1}", data.PayloadNames[i], data.PayloadString(i));
            }

            return 0;
        }
    }
}
