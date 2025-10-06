using HelpDeskTracker.Application.Logic.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskTracker.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class UsersController : BaseController
    {
        public UsersController(ILogger<UsersController> logger, IMediator mediator) : base(logger, mediator)
        {
                
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request model)
        {
            var createAccountResult = await _mediator.Send(model);
            return Ok(createAccountResult);
        }

    }
}
