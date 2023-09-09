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
      throwConfigExceptions="true">

    <targets>
        <target name="target1" type="Elmah" />
    </targets>

    <rules>
        <logger name="*" minlevel="Info" writeTo="target1" />
    </rules>
</nlog>
```

## Options
- **Layout** - Used for rendering the `Elmah.Message`. Default is `${message}`
- **LogType** - Layout for rendering the `Elmah.Type` Field. Default is `${exception:format=Type:whenEmpty=${level}}`
- **LogSource** - Layout for rendering the `Elmah.Source` Field. Default is `${exception:format=Source:whenEmpty=${logger}}`
- **LogDetail** - Layout for rendering the `Elmah.Detail` Field. Default is `${exception:format=ToString}`
- **LogHostName** - Layout for rendering the `Elmah.HostName` Field. Default is `${hostname}`
- **LogUser** - Layout for rendering the `Elmah.User` Field. Default is blank.
- **IdentityNameAsUser** - Use HttpContext.User as fallback when `LogUser` gives blank value. Default is `false`

## Notes
Not strong named (SNK) because [the dependency](https://www.nuget.org/packages/elmah.corelibrary/) isn't strong named.   
