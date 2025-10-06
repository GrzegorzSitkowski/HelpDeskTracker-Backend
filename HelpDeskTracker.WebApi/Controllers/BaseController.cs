using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskTracker.WebApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IMediator _mediator;

        protected BaseController(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
    }
}
