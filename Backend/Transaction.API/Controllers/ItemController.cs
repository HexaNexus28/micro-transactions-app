using Microsoft.AspNetCore.Mvc;
using Transaction.Business.Services;
using Transaction.Core.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/<ItemController>
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var result = await _itemService.GetAllItemsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);

        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int userId)
        {
            var result = await _itemService.GetItemByIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else return StatusCode(result.StatusCode, result);

        }

        // POST api/<ItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
