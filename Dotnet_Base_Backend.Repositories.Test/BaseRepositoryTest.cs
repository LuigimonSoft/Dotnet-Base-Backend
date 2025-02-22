using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Moq;
using Dotnet_Base_Backend.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dotnet_Base_Backend.Repositories.Test
{
    [TestClass]
    public class BaseRepositoryTest
    {
        private BaseRepository _baseRepository;
        private BaseRepository _baseRepositoryErrors;
        private MessagesDbContext? _messagesDbContext;
        private Mock<MessagesDbContext> _messagesErrorsDbContextMock;
        private Mock<DbSet<Message>> _messagesMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _messagesDbContext = DbContextFactory.Create<MessagesDbContext>("Messages" + Guid.NewGuid().ToString());
            if(_messagesDbContext == null)
                throw new Exception("Failed to create DbContext");
            
            _baseRepository = new BaseRepository(_messagesDbContext);

            _messagesMock = new Mock<DbSet<Message>>();
           

            _messagesErrorsDbContextMock = new Mock<MessagesDbContext>();

            _messagesErrorsDbContextMock.Setup(x => x.Set<Message>())
                .Returns(_messagesMock.Object);

            
            
            
            _baseRepositoryErrors = new BaseRepository(_messagesErrorsDbContextMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _messagesDbContext?.Dispose();
        }

        [TestMethod]
        public async Task GetMessage_ShouldReturnAllMessages()
        {
            // Arrange
            string messageExpected = "Hello World";
            int messageIdExpected = 1;
            await AddMessagesDataBase();

            // Act
            var result = await _baseRepository.GetMessage();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(messageExpected, result[0]?.Content);
            Assert.AreEqual(messageIdExpected, result[0]?.Id);
        }

        [TestMethod]
        public async Task GetMessage_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;
            _messagesMock.As<IQueryable<Message>>()
                .Setup(m => m.Provider)
                .Throws(new Exception("Error to connect to data base"));
            _messagesErrorsDbContextMock.Setup(x => x.Add(It.IsAny<Message>())).Throws(new Exception("Error to add messages"));

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.GetMessage();
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task AddMessage_ShouldAddNewMessageReturnNewMessage()
        {
            // Arrange
            string messageExpected = "Hello World2";

            // Act
            var result = await _baseRepository.AddMessage(messageExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(messageExpected, result.Content);
            Assert.AreEqual(1,result.Id);
        }

        [TestMethod]
        public async Task AddMessage_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.AddMessage("test message");
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldReturnMessageByText()
        {
            // Arrange
            string messageExpected = "Hello World";
            await AddMessagesDataBase();

            // Act
            var result = await _baseRepository.SearchMessage("Hello");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(messageExpected, result[0].Content);
        }

        [TestMethod]
        public async Task SearchMessage_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.SearchMessage("test");
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task GetMessageById_ShouldReturnMessageById()
        {
            // Arrange
            string messageExpected = "Hello World";
            int messageIdExpected = 1;
            await AddMessagesDataBase();

            // Act
            var result = await _baseRepository.GetMessageById(messageIdExpected);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(messageIdExpected, result.Id);
            Assert.AreEqual(messageExpected, result.Content);
        }

        [TestMethod]
        public async Task GetMessageById_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.GetMessageById(2);
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }
        [TestMethod]
        public async Task UpdateMessage_ShouldReturnMessageById()
        {
            // Arrange
            string messageExpected = "Hello World Updated";
            string messageUpdated = "Hello World";
            await AddMessagesDataBase();
            var actualMessage = (_messagesDbContext?.Messages.Where(x=> x.Content== messageUpdated).FirstOrDefaultAsync().Result) ?? throw new Exception("Message not found");
            actualMessage.Content = messageExpected;

            // Act
            var result = await _baseRepository.UpdateMessage(actualMessage);

            // Assert
            Assert.IsTrue(result);
            var messageRes = (_messagesDbContext?.Messages.FindAsync(actualMessage.Id).Result) ?? throw new Exception("Message not found");
            Assert.AreEqual(messageExpected, messageRes.Content);
        }

        [TestMethod]
        public async Task UpdateMessage_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;
            _messagesErrorsDbContextMock.Setup(x => x.Update(It.IsAny<Message>())).Throws(new Exception("Error to update messages"));

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.UpdateMessage(new Message() { Id = 1 , Content= "Hello"});
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task UpdateMessage_ShouldReturnRepositoryExceptionNotFound()
        {
            ErrorCode errorCodeException = ErrorCode.NOT_FOUND;
         


            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepository.UpdateMessage(new Message() { Id = 2, Content = "Hello" });
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnMessageById()
        {
            // Arrange
            int messageIdExpected = 1;
            await AddMessagesDataBase();
            var totalMessages = _messagesDbContext?.Messages.Count();
            // Act
            var result = await _baseRepository.DeleteMessage(messageIdExpected);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(totalMessages - 1, _messagesDbContext?.Messages.Count());
            Assert.IsNull(_messagesDbContext?.Messages.FindAsync(messageIdExpected).Result);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnRepositoryException()
        {
            ErrorCode errorCodeException = ErrorCode.DATABASE_ERROR;
            _messagesErrorsDbContextMock.Setup(x => x.Remove(It.IsAny<Message>())).Throws(new Exception("Error to remove messages"));

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepositoryErrors.DeleteMessage(2);
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        [TestMethod]
        public async Task DeleteMessage_ShouldReturnRepositoryExceptionNotFound()
        {
            ErrorCode errorCodeException = ErrorCode.NOT_FOUND;
          

            var res = await Assert.ThrowsExceptionAsync<RepositoryException>(() =>
            {
                return _baseRepository.DeleteMessage(2);
            });

            Assert.IsNotNull(res);
            Assert.AreEqual(errorCodeException, res.ErrorCode);
        }

        private async Task AddMessagesDataBase()
        {
            if (_messagesDbContext == null) throw new Exception("DbContext is null");

            _ = await _messagesDbContext.AddAsync<Message>(new Message { Content = "Hello World" });
            _ = await _messagesDbContext.AddAsync<Message>(new Message { Content = "Data 2" });
            _ = await _messagesDbContext.AddAsync<Message>(new Message { Content = "Data 3" });

            if ((await _messagesDbContext.SaveChangesAsync()) == 0) throw new Exception("Error to save test data");
        }
    }
}
