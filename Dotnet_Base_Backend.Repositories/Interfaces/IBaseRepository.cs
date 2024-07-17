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
        Task<MessagesModel> GetMessage();
        Task<MessagesModel> SetMessage(string message);
        Task<MessagesModel> SearchMessage(string message);
    }
}
