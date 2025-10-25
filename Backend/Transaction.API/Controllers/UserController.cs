using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetUserById( int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);

        }
        // POST api/<UserController>
        [HttpPost("register")]
        public void Post([FromBody] string value)
        {

        }

        [HttpPost("login")]
        public void Posty([FromBody] string value)
        {
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
