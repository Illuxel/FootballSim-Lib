namespace FootBalLife.Database.Models
{
    public class Coach
    {
        public string? PersonID { get; set; }
        public virtual Person? Person { get; set; }
    }
}