namespace FootBalLife.Database.Entities
{
    internal class EScout
    {
        public string PersonID { get; internal set; } = null!;

        public virtual EPerson Person { get; internal set; } = null!;
    }
}