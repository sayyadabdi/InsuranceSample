using Core.Entities;
using Domain.Dtos;
using EF.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            });
        }
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, RequestDto>
    {
        private readonly DbService _dbService;

        public CreateRequestCommandHandler(DbService dbService) => _dbService = dbService;

        public async Task<RequestDto> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var coverages = _dbService.Coverages.AsNoTracking().ToDictionary(k => k.Type, v => v);

            if (request.Coverages.Any(c => c.Amount < coverages[c.Coverage].Min || c.Amount > coverages[c.Coverage].Max))
                throw new ValidationException("AMOUNT IS INVALID!");

            var r = new Request()
            {
                Title = request.Title,
                EvaluationResult = new RequestEvaluation()
                {
                    Result = request.Coverages.Sum(c => c.Amount * coverages[c.Coverage].Coefficient)
                },
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
                EvaluationResult = new RequestEvaluationDto()
                {
                    Id = r.EvaluationResult.Id,
                    Result = r.EvaluationResult.Result
                },
                Coverages = r.Coverages.Select(c => new RequestCoverageDto()
                {
                    Amount = c.Amount,
                    Coverage = c.Coverage
                }).ToList()
            };
        }
    }
}
