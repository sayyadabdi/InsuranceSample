namespace Domain.Dtos
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RequestEvaluationDto EvaluationResult { get; set; }
        public List<RequestCoverageDto> Coverages { get; set; }
    }

    public class RequestCoverageDto
    {
        public decimal Amount { get; set; }
        public CoverageType Coverage { get; set; }
    }
}
