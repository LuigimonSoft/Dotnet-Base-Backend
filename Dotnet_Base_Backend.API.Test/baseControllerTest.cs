using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Dotnet_Base_Backend.API.Controllers;
using Dotnet_Base_Backend.Services.Interfaces;
using Dotnet_Base_Backend.DTO;
using Microsoft.AspNetCore.Mvc;
using Dotnet_Base_Backend.Common.Errors;
using FluentValidation;

namespace Dotnet_Base_Backend.API.Test
{
    [TestClass]
    public class BaseControllerTest
    {
        private Mock<IBaseService> _baseServiceMock;
        private BaseController _baseController;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseServiceMock = new Mock<IBaseService>();
            _baseController = new BaseController(_baseServiceMock.Object);
        }

        [TestMethod]
        public async Task GetMessage_ShouldReturnAllMessages()
        {
            // Arrange
            string messageExpected = "Hello World";
            int idExpected = 1;
            MessageDto messageDTO = new MessageDto(idExpected, messageExpected);
            _baseServiceMock.Setup(x => x.GetMessage()).ReturnsAsync([messageDTO]);

            // Act
            var response = await _baseController.GetMessage();
            ObjectResult? result = response as ObjectResult;

            List<MessageDto>? resultValue = result?.Value as List<MessageDto>;


            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(1, resultValue.Count);
            Assert.AreEqual(idExpected, resultValue[0].Id);
            Assert.AreEqual(messageExpected, resultValue[0].Message);
        }

        [TestMethod]
        public async Task GetMessage_ShouldReturnThrowException()
        {
            // Arrange
            ErrorCode errorCodeExpected = ErrorCode.INTERNAL_SERVER_ERROR;
            _baseServiceMock.Setup(x => x.GetMessage()).ThrowsAsync(new ServicesException(errorCodeExpected));

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ServicesException>(async () =>
            {
                var response = await _baseController.GetMessage();
            });


            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(errorCodeExpected, taskResult.ErrorCode);
        }

        [TestMethod]
        public async Task AddMessage_ShouldReturnMessage()
        {
            // Arrange
            string messageExpected = "Hello World2";
            MessageDto messageDTO = new MessageDto(0, messageExpected);
            _baseServiceMock.Setup(x => x.AddMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var response = await _baseController.AddMessage(messageDTO);
            ObjectResult? result = response as ObjectResult;

            MessageDto? resultValue = result?.Value as MessageDto;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(messageExpected, resultValue.Message);
        }

        [TestMethod]
        [DataRow(null, ErrorCode.REQUIRED)]
        [DataRow("", ErrorCode.EMPTY)]
        [DataRow("1234567890123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
        public async Task AddMessage_ShouldReturnThrowsException(string messageExpected, ErrorCode ErrorCodeExpected)
        {
            // Arrange
            MessageDto messageDTO = new MessageDto(1, messageExpected);
            _baseServiceMock.Setup(x => x.AddMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                var response = await _baseController.AddMessage(messageDTO);
            });


            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(((int)ErrorCodeExpected).ToString(), taskResult.Errors.First().ErrorCode);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldReturnMessage()
        {
            // Arrange
            string messageSearch = "Hello";
            string messageExpected = "Hello World";
            MessageDto messageDTO = new MessageDto(1, messageExpected);
            _baseServiceMock.Setup(x => x.SearchMessage(messageSearch)).ReturnsAsync([messageDTO]);

            // Act
            var response = await _baseController.SearchMessage(messageSearch);
            ObjectResult? result = response as ObjectResult;

            List<MessageDto>? resultValue = result?.Value as List<MessageDto>;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(messageExpected, resultValue.First().Message);
        }

        [TestMethod]
        [DataRow(null, ErrorCode.REQUIRED)]
        [DataRow("", ErrorCode.EMPTY)]
        [DataRow("1234567890123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
        public async Task SearchMessage_ShouldReturnThrowsException(string messageExpected, ErrorCode ErrorCodeExpected)
        {
            // Arrange
            MessageDto messageDTO = new MessageDto(1, messageExpected);
            _baseServiceMock.Setup(x => x.AddMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                var response = await _baseController.SearchMessage(messageExpected);
            });


            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(((int)ErrorCodeExpected).ToString(), taskResult.Errors.First().ErrorCode);
        }


        [TestMethod]
        public async Task UpdateMessage_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            MessageDto messageDTO = new MessageDto(idExpected, messageExpected);
            _baseServiceMock.Setup(x => x.UpdateMessage(It.IsAny<MessageDto>())).ReturnsAsync(true);

            // Act
            var response = await _baseController.UpdateMessage(messageDTO);
            ObjectResult? result = response as ObjectResult;

            bool? resultValue = (bool?)result?.Value;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue);
        }

        [TestMethod]
        [DataRow(null, ErrorCode.REQUIRED)]
        [DataRow("", ErrorCode.EMPTY)]
        [DataRow("1234567890123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
        public async Task UpdateMessage_ShouldReturnThrowsException(string messageExpected, ErrorCode ErrorCodeExpected)
        {
            // Arrange
            MessageDto messageDTO = new MessageDto(1, messageExpected);
            _baseServiceMock.Setup(x => x.UpdateMessage(It.IsAny<MessageDto>())).ReturnsAsync(true);

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                var response = await _baseController.UpdateMessage(messageDTO);
            });

            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(((int)ErrorCodeExpected).ToString(), taskResult.Errors.First().ErrorCode);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnThrowsException()
        {
            // Arrange
            ErrorCode ErrorCodeExpected = ErrorCode.INTERNAL_SERVER_ERROR;
            _baseServiceMock.Setup(x => x.DeleteMessage(It.IsAny<int>())).ThrowsAsync(new ServicesException(ErrorCodeExpected));

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ServicesException>(async () =>
            {
                var response = await _baseController.DeleteMessage(1);
            });

            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual((int)ErrorCodeExpected,(int) taskResult.ErrorCode);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;

            _baseServiceMock.Setup(x => x.DeleteMessage(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var response = await _baseController.DeleteMessage(idExpected);
            ObjectResult? result = response as ObjectResult;

            object? value = result?.Value;
            bool resultValue = (bool)(value ?? false);

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue);
        }

        [TestMethod]
        public async Task SearchById_ShouldReturnMessage()
        {
            // Arrange
            int idExpected = 1;
            string messageExpected = "Hello World";
            MessageDto messageDTO = new MessageDto(idExpected, messageExpected);
            _baseServiceMock.Setup(x => x.GetMessageById(idExpected)).ReturnsAsync(messageDTO);

            // Act
            var response = await _baseController.SearchById(idExpected);
            ObjectResult? result = response as ObjectResult;

            MessageDto? resultValue = result?.Value as MessageDto;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(idExpected, resultValue.Id);
            Assert.AreEqual(messageExpected, resultValue.Message);
        }

        [TestMethod]
        public async Task SearchById_ShouldReturnThrowsException()
        {
            // Arrange
            int idExpected = 1;
            ErrorCode ErrorCodeExpected = ErrorCode.INTERNAL_SERVER_ERROR;
            _baseServiceMock.Setup(x => x.GetMessageById(idExpected)).ThrowsAsync(new ServicesException(ErrorCodeExpected));

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ServicesException>(async () =>
            {
                var response = await _baseController.SearchById(idExpected);
            });

            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(ErrorCodeExpected, taskResult.ErrorCode);
        }

        [TestMethod]
        public async Task SearchById_ShouldReturnNotFound()
        {
            // Arrange
            int idExpected = 1;
            _baseServiceMock.Setup(x => x.GetMessageById(idExpected)).ReturnsAsync(null as MessageDto);

            // Act
            var response = await _baseController.SearchById(idExpected);
            NotFoundResult? result = response as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
