// Copyright 2013 Kim Christensen, Todd Meinershagen, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;
using System.Web;
using Elmah;
using NLog.Layouts;
using NLog.Targets;

namespace NLog.Elmah
{
    /// <summary>
    /// Write messages to Elmah.
    /// </summary>
    [Target("Elmah")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        private readonly ErrorLog _errorLog;

        /// <summary>
        /// Use <see cref="LogEventInfo.Level"/> as type if <see cref="LogEventInfo.Exception"/> is <c>null</c>.
        /// </summary>
        [Obsolete("Instead configure LogType Layout Property")]
        public bool LogLevelAsType { get; set; }

        /// <summary>
        /// Override <see cref="Error.Type"/>
        /// </summary>
        /// <remarks>
        /// Default: ${exception:format=Type:whenEmpty=${level}}
        /// </remarks>
        public Layout LogType { get; set; }

        /// <summary>
        /// Override <see cref="Error.Source"/>
        /// </summary>
        /// <remarks>
        /// Default: ${exception:format=Source:whenEmpty=${logger}}
        /// </remarks>
        public Layout LogSource { get; set; }

        /// <summary>
        /// Override <see cref="Error.Detail"/>
        /// </summary>
        /// <remarks>
        /// Default: ${exception:format=ToString}
        /// </remarks>
        public Layout LogDetail { get; set; }

        /// <summary>
        /// Override <see cref="Error.HostName"/>
        /// </summary>
        /// <remarks>
        /// Default: ${hostname}
        /// </remarks>
        public Layout LogHostName { get; set; }

        /// <summary>
        /// Override <see cref="Error.User"/>
        /// </summary>
        /// <remarks>
        /// Fallback to HttpContext.Current.User when <see cref="IdentityNameAsUser"/> = true
        /// </remarks>
        public Layout LogUser { get; set; }

        /// <summary>
        /// Use <see cref="System.Security.Principal.IIdentity.Name"/> from HttpContext as user.
        /// </summary>
        public bool IdentityNameAsUser { get; set; }

        /// <summary>
        /// Target with default errorlog.
        /// </summary>
        public ElmahTarget()
            : this(ErrorLog.GetDefault(null))
        { }

        /// <summary>
        /// Target with errorLog.
        /// </summary>
        /// <param name="errorLog"></param>
        public ElmahTarget(ErrorLog errorLog)
        {
            _errorLog = errorLog;
            Layout = "${message}${onexception: - ${exception:format=Message}}";
            LogSource = "${exception:format=Source:whenEmpty=${logger}}";
            LogType = "${exception:format=Type:whenEmpty=${level}}";
            LogDetail = "${exception:format=ToString}";
            LogHostName = "${hostname}";
        }

        /// <summary>
        /// Write the event.
        /// </summary>
        /// <param name="logEvent">event to be written.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = RenderLogEvent(Layout, logEvent);
            var logType = RenderLogEvent(LogType, logEvent);
            var logSource = RenderLogEvent(LogSource, logEvent);
            var logDetail = RenderLogEvent(LogDetail, logEvent);
            if (string.IsNullOrEmpty(logDetail))
                logDetail = logMessage;
            var logHostName = RenderLogEvent(LogHostName, logEvent);
            var logUser = RenderLogEvent(LogUser, logEvent);

            var httpContext = HttpContext.Current;
            if (string.IsNullOrEmpty(logUser) && IdentityNameAsUser)
                logUser = httpContext?.User?.Identity?.Name;

            var error = logEvent.Exception == null ? new Error() : httpContext != null ? new Error(logEvent.Exception, httpContext) : new Error(logEvent.Exception);
            error.Type = logType;
            error.Message = logMessage;
            error.Time = GetCurrentDateTime == null ? logEvent.TimeStamp : GetCurrentDateTime();
            error.HostName = logHostName;
            error.Detail = logDetail;
            error.Source = logSource;
            if (!string.IsNullOrEmpty(logUser))
                error.User = logUser;
            _errorLog.Log(error);
        }

        /// <summary>
        /// Method for retrieving current date and time. If <c>null</c>, then <see cref="LogEventInfo.TimeStamp"/> will be used.
        /// </summary>
        public Func<DateTime> GetCurrentDateTime { get; set; }
    }
}
