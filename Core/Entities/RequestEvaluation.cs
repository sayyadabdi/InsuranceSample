namespace Core.Entities
{
    public class RequestEvaluation
    {
        public int Id { get; set; }
        public decimal Result { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
    }
}
