# NLog.Elmah
[![Version](https://badge.fury.io/nu/NLog.ELMAH.svg)](https://www.nuget.org/packages/NLog.ELMAH)
[![AppVeyor](https://img.shields.io/appveyor/ci/nlog/nlog-Elmah/master.svg)](https://ci.appveyor.com/project/nlog/nlog-Elmah/branch/master)

ELMAH target for NLog

Extensions to [NLog](https://github.com/NLog/NLog/)

# Usage

Install the library with Nuget

>  Install-Package NLog.Elmah 

Update NLog to the latest version

> Update-Package NLog

Read the  [NLog tutorial](https://github.com/NLog/NLog/wiki/Tutorial)

Example config:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      internalLogFile="c:\temp\nlog-interal.txt" internalLogLevel="Info"
      >

    <targets>
        <target name="target1" xsi:type="Elmah" LogLevelAsType"false"  />
    </targets>

    <rules>
        <logger name="*" minlevel="Info" writeTo="target1" />
    </rules>
</nlog>

```

config:
- `LogLevelAsType`: Use Level as type if logged Exception is `null`.


Check the internal log (c:\temp\nlog-interal.txt) in case of problems

Also, users can [safely ignore the warning](https://stackoverflow.com/a/39311279/201303) it throws for that custom target:

> `This is an invalid xsi:type 'http://www.nlog-project.org/schemas/NLog.xsd:Elmah'`

## Notes
Not strong named (SNK) because [the dependency](https://www.nuget.org/packages/elmah.corelibrary/) isn't strong named.   
