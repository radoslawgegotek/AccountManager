using AccountManager.Domain.Dto;
using AccountManager.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IAppUserService _userService;

        public AppUserController(IAppUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("GetAll")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            if (result == null)
            {
                return NoContent();
            }
            return result is OkObjectResult ? Ok(result) : BadRequest(result);
        }


        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var result = await _userService.RegisterAsync(registerRequestDTO);
            return result is OkObjectResult ? Ok(result) : BadRequest(result);
        }


        [HttpPost("GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromBody] LoginRequestDTO userRequestDTO)
        {
            var result = await _userService.LoginAsync(userRequestDTO);
            return result is OkObjectResult ? Ok(result) : Unauthorized(result);
        }
    }
}
