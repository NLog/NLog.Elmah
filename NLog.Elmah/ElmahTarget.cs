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
		public bool LogLevelAsType { get; set; }

        /// <summary>
        /// Use <see cref="LogEventInfo.LoggerName"/> as source if <see cref="LogEventInfo.Exception"/> is <c>null</c>.
        /// </summary>
        public Layout Source { get; set; }

        /// <summary>
        /// Use <see cref="System.Security.Principal.IIdentity.Name"/> as user.
        /// </summary>
        public bool IdentityNameAsUser { get; set; }

        /// <summary>
        /// Target with default error log.
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
            LogLevelAsType = false;
            Source = "${exception:format=Source:whenEmpty=${logger}}";
        }

        /// <summary>
        /// Method for retrieving current date and time. If <c>null</c>, then <see cref="LogEventInfo.TimeStamp"/> will be used.
        /// </summary>
        public Func<DateTime> GetCurrentDateTime { get; set; }

        /// <summary>
        /// Write the event.
        /// </summary>
        /// <param name="logEvent">event to be written.</param>
		protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);

            var httpContext = HttpContext.Current;
            var error = CreateError(logEvent, httpContext);
            error.Type = GetType(logEvent, error);
            error.Message = logMessage;
            error.Time = GetCurrentDateTime?.Invoke() ?? logEvent.TimeStamp;
            error.HostName = Environment.MachineName;
            error.Detail = logEvent.Exception == null ? logMessage : logEvent.Exception.StackTrace;
            error.Source = Source.Render(logEvent);
            error.User = GetUser(httpContext);

            _errorLog.Log(error);
        }

        private string GetType(LogEventInfo logEvent, Error error)
        {
            return error.Exception != null
                ? error.Exception.GetType().FullName
                : LogLevelAsType ? logEvent.Level.Name : string.Empty;
        }

        private static Error CreateError(LogEventInfo logEvent, HttpContext httpContext)
        {
            if (logEvent.Exception == null)
            {
                return new Error();
            }

            if (httpContext != null)
            {
                return new Error(logEvent.Exception, httpContext);
            }

            return new Error(logEvent.Exception);
        }

        private string GetUser(HttpContext httpContext)
        {
            return IdentityNameAsUser
                ? httpContext?.User?.Identity?.Name ?? string.Empty
                : string.Empty;
        }


    }
}
