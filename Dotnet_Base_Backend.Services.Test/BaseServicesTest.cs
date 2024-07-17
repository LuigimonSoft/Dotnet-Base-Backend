using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Dotnet_Base_Backend.Services;
using Dotnet_Base_Backend.Services.Interfaces;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.DTO;

namespace Dotnet_Base_Backend.Services.Test
{
    [TestClass]
    public class BaseServicesTest
    {
        private Mock<IBaseRepository> _baseRepositoryMock;
        private IBaseService _baseService;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseRepositoryMock = new Mock<IBaseRepository>();
            _baseService = new BaseService(_baseRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetMessageTest_Valid()
        {
            // Arrange
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.GetMessage()).ReturnsAsync(new MessagesModel { messageExpected });

            // Act
            var result = await _baseService.GetMessage();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDTO>));
            Assert.AreEqual(messageExpected, result.First().Message);
        }

        [TestMethod]
        public async Task SetMessage_valid()
        {
            // Arrange
            string messageExpected = "Hello World2";
            var messages = new MessagesModel();
            messages.Add(messageExpected);
            _baseRepositoryMock.Setup(x => x.SetMessage(messageExpected)).ReturnsAsync(messages);

            // Act
            var result = await _baseService.SetMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MessageDTO));
            Assert.AreEqual(messageExpected, result.Message);
        }

        [TestMethod]
        public async Task SearchMessage_valid()
        {
            // Arrange
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.SearchMessage(messageExpected)).ReturnsAsync(new MessagesModel { messageExpected });

            // Act
            var result = await _baseService.SearchMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDTO>));
            Assert.AreEqual(messageExpected, result.First().Message);
        }
    }
}
