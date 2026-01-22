using Microsoft.AspNetCore.Mvc;
using Transaction.Core.Dtos.Request;
using Transaction.Core.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController (IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
           var result = await _userService.GetAllUsersAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode( result.StatusCode, result);

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById( int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);

        }
        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto dto)
        {
           var result = await _userService.CreateUserAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginRequestDto dto)
        {
            var result = await _userService.LoginWithTokenAsync(dto) ;
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);
        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }



        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
