using System;
using Elmah;
using NLog.Config;
using NLog.Layouts;
using NUnit.Framework;

namespace NLog.Elmah.Tests
{
	public abstract class given_target_with_identitynameasuser_turned_on
	{
		protected ErrorLog ErrorLog;
		protected readonly DateTime Now = new DateTime(2013, 10, 8, 19, 5, 0);

		[OneTimeSetUp]
		public void Init()
		{
			ErrorLog = new MemoryErrorLog(1);
			var loggingConfiguration = new LoggingConfiguration();
			var target = new ElmahTarget(ErrorLog)
			{
				IdentityNameAsUser = true,
				Layout = new SimpleLayout("${level}-${message}"),
				GetCurrentDateTime = () => Now
			};

			loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
			loggingConfiguration.AddTarget("Elmah", target);
			LogManager.Configuration = loggingConfiguration;
		}
	}
}