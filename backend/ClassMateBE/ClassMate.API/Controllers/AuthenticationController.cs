using AutoMapper;
using ClassMate.API.Models;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClassMate.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserModel model)
        {
            var dto = _mapper.Map<UserDto>(model);

            await _userService.CreateUserAsync(dto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var loginResponse = await _userService.LoginAsync(model.Email, model.Password);

            return Ok(loginResponse);
        }

    }
}
