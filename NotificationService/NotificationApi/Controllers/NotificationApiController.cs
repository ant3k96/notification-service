using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Api.Controllers.ErrorResponse;
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
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 500, type: typeof(HttpErrorResponse))]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest body)
        {
            await _mediator.Publish(body);
            return Ok();
        }
    }
}
