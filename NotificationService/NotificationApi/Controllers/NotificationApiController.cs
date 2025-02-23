using FluentValidation;
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
        private readonly IValidator<SendMessageRequest> _messageValidator;
        private readonly IMediator _mediator;

        public NotificationApiController(ILogger<NotificationApiController> logger, IValidator<SendMessageRequest> validator, IMediator mediator)
        {
            _logger = logger;
            _messageValidator = validator;
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
            var validationResult = await _messageValidator.ValidateAsync(body);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mediator.Publish(body);
            return Ok();
        }
    }
}
