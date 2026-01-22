using Microsoft.AspNetCore.Mvc;
using Transaction.Core.Dtos.Request;
using Transaction.Core.DTOs.Response;
using Transaction.Core.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IAuthTokenService _authtokenService;
        private readonly ITransactService _transactService;

        public TransactionController(IAuthTokenService authtokenService, ITransactService transactService)
        {
            _authtokenService = authtokenService;
            _transactService = transactService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
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

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public string GetById(int id)
        {
            return "value";
        }
        [HttpGet("user/{id}")]
        public string GetByUserId(int id)
        {
            return "value";
        }

        // POST api/<TransactionController>
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactRequestDto dto)
        {
            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }
            var result = await _transactService.CreateTransactionAsync(dto);
            var interop = await _authtokenService.CreateAuthTokenAsync();
            if (result.Success && interop.Success)
            {
                return Ok(result);
            }else
            {
                return StatusCode(result.StatusCode, result);
            }

        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
