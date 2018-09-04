# IIS ETW Monitors

IIS ETW monitors is a console app to perform real-time monitoring of:
* IIS request logging ETW events - the same information captured by IIS request log.
* IIS request tracing ETW events - the same information captured by IIS failed request tracing.

### Using IIS ETW Monitors

**IISETWMonitors.exe must be run as administrator.**

Monitoring IIS request logging ETW events

```
IISETWMonitors.exe
```
Or
```
IISETWMonitors.exe -l
```

Monitoring IIS request tracing ETW events
```
IISETWMonitors.exe -t
```