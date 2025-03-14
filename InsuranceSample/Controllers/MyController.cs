using Domain.Commands;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceSample.Controllers
{
    [ApiController]
    [Route("api")]
    public class MyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("requests")]
        public Task<List<RequestDto>> GetRequestsAsync(CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetRequestsCommand(), cancellationToken);
        }

        [HttpPost]
        [Route("requests")]
        public Task<RequestDto> CreateRequestAsync(CreateRequestCommand command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
