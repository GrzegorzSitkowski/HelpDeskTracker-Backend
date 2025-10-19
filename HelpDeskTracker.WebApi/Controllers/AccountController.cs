using HelpDeskTracker.Application.Logic.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskTracker.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        public AccountsController(ILogger<AccountsController> logger,
            IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentAccount()
        {
            var data = await _mediator.Send(new CurrentAccountQuery.Request() { });
            return Ok(data);
        }

    }
}
