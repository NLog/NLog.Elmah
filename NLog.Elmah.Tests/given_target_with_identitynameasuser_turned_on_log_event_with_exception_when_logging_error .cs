using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using NUnit.Framework;

namespace NLog.Elmah.Tests
{
	[TestFixture]
	public class given_target_with_identitynameasuser_turned_on_log_event_with_exception_when_logging_error : given_target_with_identitynameasuser_turned_on
	{
        private string _host;

        [Test]
        public void should_set_user()
        {
            HttpContext.Current = GetContext();
            LogManager.GetLogger("Test").Error(new Exception(), "An exception");
            var error = ErrorLog.GetFirstError();
			Assert.That(error.User, Is.EqualTo("UserName"));
		}

        private HttpContext GetContext()
        {
            const string appVirtualDir = "/";
            const string appPhysicalDir = @"c:\\projects\\SubtextSystem\\Subtext.Web\\";
            const string page = "application/default.aspx";
            _host = Environment.MachineName;
            var query = string.Empty;
            TextWriter output = null;

            var workerRequest = new SimulatedHttpRequest(appVirtualDir, appPhysicalDir, page, query, output, _host);
            var ctx = new HttpContext(workerRequest)
            {
                User = new GenericPrincipal(new GenericIdentity("UserName"), new string[0])
            };
            return ctx;
        }
	}
}