namespace FootBalLife.Database.Models
{
    public class Scout
    {
        public string? PersonID { get; set; }
        public virtual Person? Person { get; set; }
    }
}