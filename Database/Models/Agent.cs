namespace FootBalLife.Database.Models
{
    public class Agent
    {
        public string? PersonID { get; set; }
        public virtual Person? Person { get; set; }
    }
}