using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Moq;

namespace Dotnet_Base_Backend.Repositories.Test
{
    [TestClass]
    public class BaseRepositoryTest
    {
        private BaseRepository _baseRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseRepository = new BaseRepository();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            
        }

        [TestMethod]
        public async Task GetMessageTest_Valid()
        {
            // Arrange
            string messageExpected = "Hello World";

            // Act
            var result = await _baseRepository.GetMessage();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(messageExpected, result.First());
        }

        [TestMethod]
        public async Task SetMessage_valid()
        {
            // Arrange
            string messageExpected = "Hello World2";
            var messages = new MessagesModel();
            messages.Add(messageExpected);

            // Act
            var result = await _baseRepository.SetMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(messageExpected, result[0]);
        }

        [TestMethod]
        public async Task SearchMessage_valid()
        {
            // Arrange
            string messageExpected = "Hello World";
            
            // Act
            var result = await _baseRepository.SearchMessage("Hello");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(messageExpected, result.First());
        }
    }
}
