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
        public async Task GetMessageTest_Valid()
        {
            // Arrange
            string messageExpected = "Hello World";
            MessageDTO messageDTO = new MessageDTO { Message = messageExpected };
            _baseServiceMock.Setup(x => x.GetMessage()).ReturnsAsync(new List<MessageDTO> { messageDTO });

            // Act
            var response = await _baseController.GetMessage();
            ObjectResult result = response as ObjectResult;

            List<MessageDTO> resultValue = result.Value as List<MessageDTO>;


            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(messageExpected, resultValue.First().Message);
        }

        [TestMethod]
        public async Task GetMessageTest_Invalid_exception()
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
        public async Task SetMessage_valid()
        {
            // Arrange
            string messageExpected = "Hello World2";
            MessageDTO messageDTO = new MessageDTO { Message = messageExpected };
            _baseServiceMock.Setup(x => x.SetMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var response = await _baseController.SetMessage(messageDTO);
            ObjectResult result = response as ObjectResult;

            MessageDTO resultValue = result.Value as MessageDTO;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(messageExpected, resultValue.Message);
        }

        [TestMethod]
        [DataRow(null, ErrorCode.REQUIRED)]
        [DataRow("", ErrorCode.EMPTY)]
        [DataRow("1234567890123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
        public async Task SetMessage_Invalid(string messageExpected, ErrorCode ErrorCodeExpected)
        {
            // Arrange
            MessageDTO messageDTO = new MessageDTO { Message = messageExpected };
            _baseServiceMock.Setup(x => x.SetMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                var response = await _baseController.SetMessage(messageDTO);
            });
            

            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(((int) ErrorCodeExpected).ToString(), taskResult.Errors.First().ErrorCode);
        }

        [TestMethod]
        public async Task SearchMessage_valid()
        {
            // Arrange
            string messageSearch = "Hello";
            string messageExpected = "Hello World";
            MessageDTO messageDTO = new MessageDTO { Message = messageExpected };
            _baseServiceMock.Setup(x => x.SearchMessage(messageSearch)).ReturnsAsync(new List<MessageDTO> { messageDTO });

            // Act
            var response = await _baseController.SearchMessage(messageSearch);
            ObjectResult result = response as ObjectResult;

            List<MessageDTO> resultValue = result.Value as List<MessageDTO>;

            // Assert
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(messageExpected, resultValue.First().Message);
        }

        [TestMethod]
        [DataRow(null, ErrorCode.REQUIRED)]
        [DataRow("", ErrorCode.EMPTY)]
        [DataRow("1234567890123456789012345678901234567890", ErrorCode.MAX_LENGTH)]
        public async Task SearchMessage_Invalid(string messageExpected, ErrorCode ErrorCodeExpected)
        {
            // Arrange
            MessageDTO messageDTO = new MessageDTO { Message = messageExpected };
            _baseServiceMock.Setup(x => x.SetMessage(messageExpected)).ReturnsAsync(messageDTO);

            // Act
            var taskResult = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                var response = await _baseController.SearchMessage(messageExpected);
            });


            // Assert
            Assert.IsNotNull(taskResult);
            Assert.AreEqual(((int)ErrorCodeExpected).ToString(), taskResult.Errors.First().ErrorCode);
        }
    }
}
