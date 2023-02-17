namespace FootBalLife.Database.Entities
{
    internal class ECoach
    {
        public string? PersonID { get; set; }
        public virtual EPerson? Person { get; internal set; }
    }
}