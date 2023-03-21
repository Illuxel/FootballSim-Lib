namespace DatabaseLayer
{
    public class Contract
    {
        public string? Id { get; internal set; }

        public string? TeamId { get; set; }
        public Team? Team { get; set; }

        public string? PersonId { get; set; }
        public Person? Person { get; set; }

        public string? SeasonTo { get; set; }
        public string? SeasonFrom { get; set; }

        public double Price { get; set; }
    }
}