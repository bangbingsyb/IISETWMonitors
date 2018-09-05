# IIS ETW Monitors

IIS ETW monitors is a console app to perform real-time monitoring of:
* IIS request logging ETW events - the same information captured by IIS request log.
* IIS request tracing ETW events - the same information captured by IIS failed request tracing.
* IIS configuration ETW events

## Syntax
```
IISETWMonitors [options]
```
**Note: IISETWMonitors.exe must be run as administrator.**

## Parameters
|Parameter|Description|  
|-------|--------|  
|-l|Monitor IIS request logging ETW events.|
|-t|Monitor IIS request tracing ETW events.|
|-c|Monitor IIS configuration ETW events.|

## Examples
* To monitor IIS request logging ETW events, run
```
IISETWMonitors.exe -l
```
or simply run
```
IISETWMonitors.exe
```
