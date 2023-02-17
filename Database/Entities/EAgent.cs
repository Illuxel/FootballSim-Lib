namespace FootBalLife.Database.Entities
{
    internal class EAgent
    {
        public string? PersonID { get; set; }
        public virtual EPerson? Person { get; internal set; }
    }
}