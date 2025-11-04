using Microsoft.AspNetCore.Mvc;
using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Responses;
using mindtechNewsletter.Server.Services;

namespace mindtechNewsletter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberService _subscriberService;

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        //POST: api/subscriber
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriberCreateDTO dto)
        {
            var response = await _subscriberService.SubscribeAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //POST: api/subscriber/unsubscribe
        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscriberCreateDTO dto)
        {
            var response = await _subscriberService.UnsubscribeAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //GET: api/subscriber/all - Used to check database records.
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _subscriberService.GetAllAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
