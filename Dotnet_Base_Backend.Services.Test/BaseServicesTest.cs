using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Dotnet_Base_Backend.Services.Interfaces;
using Moq;

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
        public async Task GetMessage_ShouldReturnAllMessages()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.GetMessage()).ReturnsAsync([new Message { Id = idExpected, Content = messageExpected }]);

            // Act
            var result = await _baseService.GetMessage();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDto>));
            Assert.AreNotSame(0, result.Count);
            Assert.AreEqual(messageExpected, result[0].Message);
            Assert.AreEqual(idExpected, result[0].Id);
        }

        [TestMethod]
        public async Task GetMessage_ShouldReturnEmptyList()
        {
            // Arrange
            _baseRepositoryMock.Setup(x => x.GetMessage()).ReturnsAsync(new List<Message>());

            // Act
            var result = await _baseService.GetMessage();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDto>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetMessage_ShouldThrowException()
        {
            // Arrange
            _baseRepositoryMock.Setup(x => x.GetMessage()).ThrowsAsync(new Exception("Error to read messages"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.GetMessage();
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }

        [TestMethod]
        public async Task AddMessage_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World2";
            var messages = new Message() { Id = idExpected, Content = messageExpected };
            
            _baseRepositoryMock.Setup(x => x.AddMessage(messageExpected)).ReturnsAsync(messages);

            // Act
            var result = await _baseService.AddMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MessageDto));
            Assert.AreEqual(idExpected, result.Id);
            Assert.AreEqual(messageExpected, result.Message);
        }

        [TestMethod]
        public async Task AddMessage_ShouldThrowException()
        {
            // Arrange
            string messageExpected = "Hello World2";
            _baseRepositoryMock.Setup(x => x.AddMessage(messageExpected)).ThrowsAsync(new Exception("Error to add message"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.AddMessage(messageExpected);
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.SearchMessage(messageExpected)).ReturnsAsync([new Message { Id= idExpected, Content = messageExpected }]);

            // Act
            var result = await _baseService.SearchMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDto>));
            Assert.AreNotSame(0, result.Count);
            Assert.AreEqual(idExpected, result[0].Id);
            Assert.AreEqual(messageExpected, result[0].Message);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldReturnEmptyList()
        {
            // Arrange
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.SearchMessage(messageExpected)).ReturnsAsync(new List<Message>());

            // Act
            var result = await _baseService.SearchMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<MessageDto>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldThrowException()
        {
            // Arrange
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.SearchMessage(messageExpected)).ThrowsAsync(new Exception("Error to search message"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.SearchMessage(messageExpected);
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }

        [TestMethod]
        public async Task GetMessageById_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            _baseRepositoryMock.Setup(x => x.GetMessageById(idExpected)).ReturnsAsync(new Message { Id = idExpected, Content = messageExpected });

            // Act
            var result = await _baseService.GetMessageById(idExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MessageDto));
            Assert.AreEqual(idExpected, result.Id);
            Assert.AreEqual(messageExpected, result.Message);
        }

        
        [TestMethod]
        public async Task GetMessageById_ShouldThrowException()
        {
            // Arrange
            int idExpected = 1;
            _baseRepositoryMock.Setup(x => x.GetMessageById(idExpected)).ThrowsAsync(new Exception("Error to get message by id"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.GetMessageById(idExpected);
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }

        [TestMethod]
        public async Task UpdateMessage_ShouldReturnTrue()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            var message = new MessageDto(idExpected, messageExpected);
            _baseRepositoryMock.Setup(x => x.UpdateMessage(It.IsAny<Message>())).ReturnsAsync(true);

            // Act
            var result = await _baseService.UpdateMessage(message);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateMessage_ShouldThrowException()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            var message = new MessageDto(idExpected, messageExpected);
            _baseRepositoryMock.Setup(x => x.UpdateMessage(It.IsAny<Message>())).ThrowsAsync(new Exception("Error to update message"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.UpdateMessage(message);
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnTrue()
        {
            // Arrange
            int idExpected = 1;
            _baseRepositoryMock.Setup(x => x.DeleteMessage(idExpected)).ReturnsAsync(true);

            // Act
            var result = await _baseService.DeleteMessage(idExpected);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldThrowException()
        {
            // Arrange
            int idExpected = 1;
            _baseRepositoryMock.Setup(x => x.DeleteMessage(idExpected)).ThrowsAsync(new Exception("Error to delete message"));

            // Act
            var res = await Assert.ThrowsExceptionAsync<ServicesException>(() =>
            {
                return _baseService.DeleteMessage(idExpected);
            });

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ErrorCode.INTERNAL_SERVER_ERROR, res.ErrorCode);
        }


    }
}
