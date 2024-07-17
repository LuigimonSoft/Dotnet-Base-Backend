using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using Dotnet_Base_Backend.Services.Interfaces;
using Dotnet_Base_Backend.API.Validators;
using FluentValidation;

namespace Dotnet_Base_Backend.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IBaseService _baseService;

        public BaseController(IBaseService baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MessageDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMessage()
        {
            return StatusCode(StatusCodes.Status200OK, await _baseService.GetMessage());
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<MessageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetMessage(MessageDTO message)
        {
            MessageDTOValidator validator = new MessageDTOValidator();
            validator.ValidateAndThrow(message);

            var res = await _baseService.SetMessage(message.Message);
            
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpGet("{message}")]
        [ProducesResponseType(typeof(List<MessageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchMessage(string message)
        {
            MessageDTO messageDTO = new MessageDTO { Message = message };
            MessageDTOValidator validator = new MessageDTOValidator();
            validator.ValidateAndThrow(messageDTO);

            var res = await _baseService.SearchMessage(messageDTO.Message);

            return StatusCode(StatusCodes.Status200OK, res);
        }
    }
}
