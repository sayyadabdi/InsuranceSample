using Domain.Dtos;
using EF.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Commands
{
    public class GetRequestsCommand : IRequest<List<RequestDto>>
    {

    }

    public class GetRequestsCommandHandler : IRequestHandler<GetRequestsCommand, List<RequestDto>>
    {
        private readonly DbService _dbService;

        public GetRequestsCommandHandler(DbService dbService) => _dbService = dbService;

        public async Task<List<RequestDto>> Handle(GetRequestsCommand request, CancellationToken cancellationToken)
        {
            return (await _dbService.Requests.Include(r => r.Coverages)
                                             .ToListAsync(cancellationToken))
                                             .Select(r => new RequestDto()
                                             {
                                                 Id = r.Id,
                                                 Title = r.Title,
                                                 Coverages = r.Coverages.Select(c => new RequestCoverageDto()
                                                 {
                                                     Amount = c.Amount,
                                                     Coverage = c.Coverage
                                                 }).ToList()
                                             }).ToList();
        }
    }
}
