using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Context;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Base_Backend.Repositories
{
    public class BaseRepository: IBaseRepository
    {
        private readonly MessagesDbContext _context;
        public BaseRepository(MessagesDbContext messagesDbContext) 
        {
            _context = messagesDbContext;
        }
        public async Task<List<Message>> GetMessage()
        {
            try
            {
                return await _context.Messages.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<Message> AddMessage(string message)
        {
            try
            {
                var messageObj = new Message()
                {
                    Content = message
                };
                _context.Messages.Add(messageObj);

                await _context.SaveChangesAsync();

                return messageObj;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<Message?> GetMessageById(int id)
        {
            try
            {
                return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<List<Message>> SearchMessage(string message)
        {
            try
            {
                return await _context.Messages.Where(x => x.Content.Contains(message, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<bool> UpdateMessage(Message message)
        {
            try
            {
                if(!await _context.Messages.AnyAsync(x => x.Id == message.Id))
                    throw new RepositoryException(ErrorCode.NOT_FOUND);
                
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }

        public async Task<bool> DeleteMessage(int id)
        {
            try
            {
                var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
                if (message == null)
                    throw new RepositoryException(ErrorCode.NOT_FOUND);
                
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ErrorCode.DATABASE_ERROR, ex);
            }
        }
    }
}
