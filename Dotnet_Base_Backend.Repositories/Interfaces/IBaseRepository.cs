using Dotnet_Base_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
