using AutoMapper;
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models.Exceptions;
using MessangerBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MessangerBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] UserLoginRegisterDTO userDto)
        {
            try
            {
                var user = await _userService.Register(userDto.Nickname, userDto.Password);
                //var jwt = JwtGenerator.GenerateJwt(user, "", DateTime.UtcNow.AddMinutes(5));
                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (UserServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LoginUser([FromBody] UserLoginRegisterDTO userDto)
        {
            try
            {
                var user = await _userService.Login(userDto.Nickname, userDto.Password);
                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (UserServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var users = _userService.GetUsers(page, size);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        [HttpGet("search/{nickname}")]
        public ActionResult<IEnumerable<UserDTO>> SearchUsers(string nickname)
        {
            var users = _userService.SearchUsers(nickname);
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }
    }
}
