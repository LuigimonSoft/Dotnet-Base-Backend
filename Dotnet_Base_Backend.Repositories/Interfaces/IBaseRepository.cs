using Dotnet_Base_Backend.Models;

namespace Dotnet_Base_Backend.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        Task<List<Message>> GetMessage();
        Task<Message> AddMessage(string message);
        Task<List<Message>> SearchMessage(string message);
        Task<Message?> GetMessageById(int id);
        Task<bool> UpdateMessage(Message message);
        Task<bool> DeleteMessage(int id);
    }
}
