using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Services.Interfaces
{
    public interface IBaseService
    {
        Task<List<MessageDTO>> GetMessage();
        Task<MessageDTO> SetMessage(string message);
        Task<List<MessageDTO>> SearchMessage(string message);
    }
}
