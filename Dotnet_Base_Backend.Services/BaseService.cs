using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Models;
using Dotnet_Base_Backend.Repositories.Interfaces;
using Dotnet_Base_Backend.Services.Interfaces;

namespace Dotnet_Base_Backend.Services
{
    public class BaseService: IBaseService
    {
        private readonly IBaseRepository _baseRepository;

        public BaseService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<List<MessageDto>> GetMessage()
        {
            try
            {
                List<MessageDto> result = new List<MessageDto>();
                var messages = await _baseRepository.GetMessage();
                messages.ForEach(message => result.Add(new MessageDto(message.Id,message.Content)));
                return result;
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<MessageDto?> AddMessage(string message)
        {
            try
            {
                var messages = await _baseRepository.AddMessage(message);

                return new MessageDto(messages.Id, messages.Content);
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<List<MessageDto>> SearchMessage(string message)
        {
            try
            {
                var messages = await _baseRepository.SearchMessage(message);
                return messages.Select(message => { return new MessageDto(message.Id, message.Content); }).ToList();
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<MessageDto?> GetMessageById(int id)
        {
            try
            {
                var message = await _baseRepository.GetMessageById(id);
                if (message != null)
                    return new MessageDto(message.Id, message.Content);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<bool> UpdateMessage(MessageDto message)
        {
            try
            {
                return await _baseRepository.UpdateMessage(new Message() { Id = message.Id, Content = message.Message });
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }

        public async Task<bool> DeleteMessage(int id)
        {
            try
            {
                return await _baseRepository.DeleteMessage(id);
            }
            catch (Exception ex)
            {
                throw new ServicesException(ErrorCode.INTERNAL_SERVER_ERROR, ex);
            }
        }
    }
}
