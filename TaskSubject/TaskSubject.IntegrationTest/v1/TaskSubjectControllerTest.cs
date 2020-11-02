using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TaskSubject.API.ModelsOutput.v1;
using TaskSubject.IntegrationTest.Contracts.v1;

namespace TaskSubject.IntegrationTest.v1
{
    [TestFixture()]
    public class TaskSubjectControllerTest : IntegrationWithDataBaseTest
    {
        [Test]
        public async Task Get_WithPostBefore_ReturnThisPostByReturnedLink()
        {
            var uid = Guid.NewGuid().ToString();
            var objectValue = $"Object: {Guid.NewGuid()}";

            var content = new StringContent(JsonSerializer.Serialize(new { Uid = uid, Object = objectValue }),
                Encoding.UTF8, "application/json");

            var responseCreate = await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var response = await Client.GetAsync(responseCreate.Headers.Location.AbsoluteUri);
            var modelJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<TaskSubjectModel>(
                modelJson,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Should().NotBeNull();
            responseObject.Uid.Should().Be(uid);
            responseObject.Object.Should().Be(objectValue);
        }

        [Test]
        public async Task Get_WithPostBefore_ReturnThisPostByUid()
        {
            var uid = Guid.NewGuid().ToString();
            var objectValue = $"Object: {Guid.NewGuid()}";

            var content = new StringContent(JsonSerializer.Serialize(new { Uid = uid, Object = objectValue }),
                Encoding.UTF8, "application/json");

            await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var response = await Client.GetAsync(ApiRoutes.TaskSubject.Get.Replace("{id}", uid));
            var modelJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<TaskSubjectModel>(
                modelJson,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Should().NotBeNull();
            responseObject.Uid.Should().Be(uid);
            responseObject.Object.Should().Be(objectValue);
        }

        [Test]
        public async Task GetAll_With2PostBefore_ReturnThisPosts()
        {
            var uid1 = Guid.NewGuid().ToString();
            var objectValue1 = $"Object: {Guid.NewGuid()}";

            var uid2 = Guid.NewGuid().ToString();
            var objectValue2 = $"Object: {Guid.NewGuid()}";

            var content1 = new StringContent(JsonSerializer.Serialize(new { Uid = uid1, Object = objectValue1 }),
                Encoding.UTF8, "application/json");
            var content2 = new StringContent(JsonSerializer.Serialize(new { Uid = uid2, Object = objectValue2 }),
                Encoding.UTF8, "application/json");

            await Client.PostAsync(ApiRoutes.TaskSubject.Create, content1);
            await Client.PostAsync(ApiRoutes.TaskSubject.Create, content2);
            var response = await Client.GetAsync(ApiRoutes.TaskSubject.GetAll);
            var modelJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<List<TaskSubjectModel>>(
                modelJson,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Should().NotBeNull();

            //For the clean database should be exactly 2
            responseObject.Count.Should().BeGreaterOrEqualTo(2);
            responseObject.Should().Contain(item => item.Uid == uid1 && item.Object == objectValue1);
            responseObject.Should().Contain(item => item.Uid == uid2 && item.Object == objectValue2);
        }

        [Test]
        public async Task Post_DefaultData_ReturnStandardAnswer()
        {
            var uid = Guid.NewGuid().ToString();
            var objectValue = $"Object: {Guid.NewGuid()}";

            var content = new StringContent(JsonSerializer.Serialize(new { Uid = uid, Object = objectValue }),
                Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var modelJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<TaskSubjectModel>(
                modelJson,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });


            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.AbsoluteUri.Should().NotBeNull();

            responseObject.Should().NotBeNull();
            responseObject.Uid.Should().Be(uid);
            responseObject.Object.Should().Be(objectValue);
        }

        [Test]
        public async Task Post_WithNoData_ReturnBadRequest()
        {
            var content = new StringContent(JsonSerializer.Serialize(new { }),
                Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var responseBody = await response.Content.ReadAsStringAsync();


            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().Contain("One or more validation errors occurred.");
            responseBody.Should().Contain("The Uid field is required.");
            responseBody.Should().Contain("The Object field is required.");
        }

        [Test]
        public async Task Post_WithNoUid_ReturnBadRequest()
        {
            //var uid = Guid.NewGuid().ToString();
            var objectValue = $"Object: {Guid.NewGuid()}";

            var content = new StringContent(JsonSerializer.Serialize(new {Object = objectValue }),
                Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var responseBody = await response.Content.ReadAsStringAsync();


            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().Contain("One or more validation errors occurred.");
            responseBody.Should().Contain("The Uid field is required.");
        }

        [Test]
        public async Task Post_WithNoObjectValue_ReturnBadRequest()
        {
            var uid = Guid.NewGuid().ToString();
            //var objectValue = $"Object: {Guid.NewGuid()}";

            var content = new StringContent(JsonSerializer.Serialize(new { Uid = uid }),
                Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(ApiRoutes.TaskSubject.Create, content);
            var responseBody = await response.Content.ReadAsStringAsync();


            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().Contain("One or more validation errors occurred.");
            responseBody.Should().Contain("The Object field is required.");
        }

      


        /*
        [Test]
        public async Task Get_()
        {
            var response = await _client.GetAsync(ApiRoutes.TaskSubject.Get.Replace());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }*/
    }
}