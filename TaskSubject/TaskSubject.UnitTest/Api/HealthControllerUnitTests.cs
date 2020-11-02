using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TaskSubject.API.Controllers;

namespace TaskSubject.UnitTest.Api
{
    [TestFixture()]
    public class HealthControllerUnitTests
    {
        private readonly Mock<ILogger<HealthController>> _loggerMock;

        public HealthControllerUnitTests()
        {
            _loggerMock = new Mock<ILogger<HealthController>>();
        }

        [Test]
        public void Get__ReturnOk()
        {
            var controller = new HealthController(_loggerMock.Object);

            controller.Get();
        }
    }
}
