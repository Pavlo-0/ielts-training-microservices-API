using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TaskSubject.IntegrationTest.Contracts;

namespace TaskSubject.IntegrationTest
{
    [TestFixture()]
    public class HealthControllerTest : IntegrationTest
    {
        [Test]
        public async Task Get__ReturnOk()
        {
            var response = await Client.GetAsync(ApiRoutes.Health.Get);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
