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
        [ProducesResponseType(typeof(List<MessageDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMessage()
        {
            return StatusCode(StatusCodes.Status200OK, await _baseService.GetMessage());
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMessage([FromBody] MessageDto messageDto)
        {
            MessageDTOValidator validator = new MessageDTOValidator();
            await validator.ValidateAndThrowAsync(messageDto);

            var res = await _baseService.AddMessage(messageDto.Message);
            
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpGet("search/{message}")]
        [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchMessage(string message)
        {
            MessageDto messageDto = new(0, message);

            MessageDTOValidator validator = new MessageDTOValidator();
            await validator.ValidateAndThrowAsync(messageDto);

            var res = await _baseService.SearchMessage(message);

            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchById(int id)
        {
            var res = await _baseService.GetMessageById(id);

            if(res is null)
                return NotFound();
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpPut()]
        [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMessage(MessageDto message)
        {
            MessageDTOValidator validator = new MessageDTOValidator(true);
            await validator.ValidateAndThrowAsync(message);

            var res = await _baseService.UpdateMessage(message);

            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IList<Error>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var res = await _baseService.DeleteMessage(id);

            return StatusCode(StatusCodes.Status200OK, res);
        }
    }
}
