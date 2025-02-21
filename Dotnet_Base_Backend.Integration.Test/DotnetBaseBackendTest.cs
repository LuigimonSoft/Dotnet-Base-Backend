using Dotnet_Base_Backend.API;
using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Integration.Test.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Dotnet_Base_Backend.Integration.Test;

[TestClass]
public class DotnetBaseBackendTest 
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task GetMessages_ShouldReturnSuccess()
    {
        await AddMessage_ReturnSuccess();
        string messageExpected = "Hello World2";
        // Arrange
        var response = await _client.GetAsync("/api/v1/base");

        // Act
        var responseString = await response.Content.ReadAsStringAsync();
        var messages = JsonConvert.DeserializeObject<List<MessageDto>>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(messages);
        Assert.AreEqual(messageExpected, messages[0].Message);
    }


    [TestMethod]
    public async Task AddMessage_ReturnSuccess()
    {
        // Arrange
        string messageExpected = "Hello World2";
        int idExpected = 1;
        var message = new MessageDto(idExpected, messageExpected);
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/base", content);

        var responseString = await response.Content.ReadAsStringAsync();
        var messages = JsonConvert.DeserializeObject<MessageDto>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(messages);
        Assert.AreEqual(messageExpected, messages.Message);
    }

    [TestMethod]
    [DataRow(1, "",ErrorCode.EMPTY)]
    [DataRow(1, "0123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
    public async Task AddMessage_ReturnBadRequest(int idExpected, string message, ErrorCode ErrorCodeExpected)
    {
        // Arrange
        var messageDTO = new MessageDto(idExpected, message);
        var content = new StringContent(JsonConvert.SerializeObject(messageDTO), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/base", content);

        var responseString = await response.Content.ReadAsStringAsync();
        var Errors = JsonConvert.DeserializeObject<List<Error>>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(Errors);
        Assert.AreEqual(ErrorCodeExpected, Errors[0].Code);
    }

    [TestMethod]
    public async Task SearchMessage_ReturnSuccess()
    {
        // Arrange
        string message = "Hello";
        string messageExpected = "Hello World2";
        await AddMessage_ReturnSuccess();
        // Act
        var response = await _client.GetAsync($"/api/v1/base/search/{message}");

        var responseString = await response.Content.ReadAsStringAsync();
        var messages = JsonConvert.DeserializeObject<List<MessageDto>>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(messages);
        Assert.AreEqual(messageExpected, messages.First().Message);
    }

    [TestMethod]
    [DataRow("0123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
    public async Task SearchMessage_ReturnBadRequest(string message, ErrorCode ErrorCodeExpected)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"/api/v1/base/search/{message}");

        var responseString = await response.Content.ReadAsStringAsync();
        var Errors = JsonConvert.DeserializeObject<List<Error>>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(Errors);
        Assert.AreEqual(ErrorCodeExpected, Errors.First().Code);
    }

    [TestMethod]
    public async Task UpdateMessage_ReturnErrorIdInvalid()
    {
        // Arrange
        string message = "Hello";
        var messageDTO = new MessageDto(0, message);
        var content = new StringContent(JsonConvert.SerializeObject(messageDTO), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v1/base", content);

        var responseString = await response.Content.ReadAsStringAsync();
        var Errors = JsonConvert.DeserializeObject<List<Error>>(responseString);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(Errors);
        Assert.AreEqual(ErrorCode.INVALID, Errors.First().Code);
    }

}