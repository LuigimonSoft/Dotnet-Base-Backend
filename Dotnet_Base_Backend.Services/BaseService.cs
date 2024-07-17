using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Dotnet_Base_Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Services
{
    public class BaseService: IBaseService
    {
        private readonly IBaseRepository _baseRepository;

        public BaseService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<List<MessageDTO>> GetMessage()
        {
            try
            {
                List<MessageDTO> result = new List<MessageDTO>();
                var messages = await _baseRepository.GetMessage();
                messages.ForEach(message => result.Add(new MessageDTO() { Message = message }));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<MessageDTO> SetMessage(string message)
        {
            try
            {
               MessageDTO result = new MessageDTO();
               var messages = await _baseRepository.SetMessage(message);
                if(messages.Count > 0)
                    result.Message = messages[0];
                return result;
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<List<MessageDTO>> SearchMessage(string message)
        {
            try
            {
                List<MessageDTO> result = new List<MessageDTO>();
                var messages = await _baseRepository.SearchMessage(message);
                messages.ForEach(message => result.Add(new MessageDTO() { Message = message }));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }
    }
}
