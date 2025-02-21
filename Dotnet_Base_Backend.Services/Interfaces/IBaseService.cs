using Dotnet_Base_Backend.DTO;

namespace Dotnet_Base_Backend.Services.Interfaces
{
    public interface IBaseService
    {
        Task<List<MessageDto>> GetMessage();
        Task<MessageDto> AddMessage(string message);
        Task<List<MessageDto>> SearchMessage(string message);
        Task<MessageDto?> GetMessageById(int id);
        Task<bool> UpdateMessage(MessageDto message);
        Task<bool> DeleteMessage(int id);

    }
}
