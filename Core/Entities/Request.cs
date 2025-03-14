namespace Core.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RequestEvaluation EvaluationResult { get; set; }
        public ICollection<RequestCoverage> Coverages { get; set; }
    }
}
