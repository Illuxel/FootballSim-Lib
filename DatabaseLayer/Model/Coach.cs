namespace DatabaseLayer
{
    public class Coach
    {
        public string? PersonID { get; internal set; }
        public virtual Person? Person { get; internal set; }
    }
}