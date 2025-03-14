namespace Core.Entities
{
    public class Coverage
    {
        public int Id { get; set; }
        public CoverageType Type { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Coefficient { get; set; }
    }
}
