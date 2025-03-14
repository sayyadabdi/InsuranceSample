namespace Core.Entities
{
    public class RequestCoverage
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CoverageType Coverage { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
