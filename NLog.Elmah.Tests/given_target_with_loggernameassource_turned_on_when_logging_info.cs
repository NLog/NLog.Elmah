using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
    [TestFixture]
    public class given_target_with_loglevelastype_not_set_and_loggernameassource_set_and_log_event_with_no_exception_when_logging_info :
            given_target_with_loggernameassource_turned_on
    {
        [SetUp]
        public void SetUp()
        {
            ErrorLog.Clear();
            var logger = LogManager.GetLogger("Test");
            logger.Info("This is an info message.");
        }

        [Test]
        public void should_set_source()
        {
            var error = ErrorLog.GetFirstError();
            Assert.That(error.Source, Is.Not.Empty);
        }
    }
}