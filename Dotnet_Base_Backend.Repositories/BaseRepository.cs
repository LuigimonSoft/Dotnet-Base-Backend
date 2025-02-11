using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Base_Backend.Repositories
{
    public class BaseRepository: IBaseRepository
    {
        private MessagesModel _messages = new MessagesModel();
        public BaseRepository() 
        {
            _messages.Add("Hello World");
        }
        public async Task<MessagesModel> GetMessage()
        {
            try
            {
                return _messages;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<MessagesModel> SetMessage(string message)
        {
            try
            {
                _messages.Add(message);
                return new MessagesModel() { _messages.Last() };
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<MessagesModel> SearchMessage(string message)
        {
            try
            {
                return new MessagesModel(_messages.Where(x => x.Contains(message, StringComparison.OrdinalIgnoreCase)).ToList());
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }
    }
}
