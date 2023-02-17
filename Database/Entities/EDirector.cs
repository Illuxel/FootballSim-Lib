namespace FootBalLife.Database.Entities
{
    internal class EDirector
    {
        public string? PersonID { get; set; }
        public virtual EPerson? Person { get; internal set; }
    }
}