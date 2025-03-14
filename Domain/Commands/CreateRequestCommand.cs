using Core.Entities;
using Domain.Dtos;
using EF.Services;
using FluentValidation;
using MediatR;

namespace Domain.Commands
{
    public class RequestCoverageModel
    {
        public decimal Amount { get; set; }
        public CoverageType Coverage { get; set; }
    }

    public class CreateRequestCommand : IRequest<RequestDto>
    {
        public string Title { get; set; }
        public List<RequestCoverageModel> Coverages { get; set; }
    }

    public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Coverages).NotNull().Must(x => x != null && x.Count > 0 && x.Count <= 3 && x.Select(x => x.Coverage).Distinct().Count() == x.Count);

            RuleForEach(x => x.Coverages).ChildRules(x =>
            {
                x.RuleFor(x => x.Coverage).NotNull().IsInEnum();
                x.RuleFor(x => x.Amount).NotNull().GreaterThan(0);

                // CAN GET FROM DB TOO
                x.RuleFor(x => x.Amount).GreaterThanOrEqualTo(5000).LessThanOrEqualTo(500000000).When(x => x.Coverage == CoverageType.Surgery);
                x.RuleFor(x => x.Amount).GreaterThanOrEqualTo(4000).LessThanOrEqualTo(400000000).When(x => x.Coverage == CoverageType.Dental);
                x.RuleFor(x => x.Amount).GreaterThanOrEqualTo(2000).LessThanOrEqualTo(200000000).When(x => x.Coverage == CoverageType.Hospital);
            });
        }
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, RequestDto>
    {
        private readonly DbService _dbService;

        public CreateRequestCommandHandler(DbService dbService) => _dbService = dbService;

        public async Task<RequestDto> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var r = new Request()
            {
                // CAN USE EF CORE AUTO INCREMENT ID
                Id = new Random().Next(0, int.MaxValue),
                Title = request.Title,
                Coverages = request.Coverages.Select(c => new RequestCoverage()
                {
                    Amount = c.Amount,
                    Coverage = c.Coverage,
                }).ToList()
            };

            await _dbService.Requests.AddAsync(r, cancellationToken);

            await _dbService.SaveChangesAsync(cancellationToken);

            return new RequestDto()
            {
                Id = r.Id,
                Title = r.Title,
                Coverages = r.Coverages.Select(c => new RequestCoverageDto()
                {
                    Amount = c.Amount,
                    Coverage = c.Coverage
                }).ToList()
            };
        }
    }
}
