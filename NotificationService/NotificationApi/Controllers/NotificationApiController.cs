using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Api.Model;
using System.Net.Mime;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class NotificationApiController : ControllerBase
    {
        private readonly ILogger<NotificationApiController> _logger;
        private readonly IMediator _mediator;

        public NotificationApiController(ILogger<NotificationApiController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("send_message")]
        [Consumes(contentType: MediaTypeNames.Application.Json)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest body)
        {
            await _mediator.Publish(body);
            return Ok();
        }
    }
}