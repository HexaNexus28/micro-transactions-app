using Microsoft.AspNetCore.Mvc;
using Transaction.Core.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTokenController : ControllerBase
    {
        private readonly IAuthTokenService _authtokenService;


        public AuthTokenController(IAuthTokenService authtokenService)
        {
            _authtokenService = authtokenService;
        }
        // GET: api/<AuthTokenController>
        [HttpGet]
        public async Task<IActionResult> GetAuthTokens()
        {
            var result = await _authtokenService.GetAllAuthTokenAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(result.StatusCode, result);
            }
        }
                // GET api/<AuthTokenController>/5
               [HttpGet("{id}")]
                public string Get(int id)
                {
                    return "value";
                }

        // POST api/<AuthTokenController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthTokenController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthTokenController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
