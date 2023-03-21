namespace DatabaseLayer
{
    public class Scout
    {
        public string? PersonID { get; internal set; }
        public virtual Person? Person { get; internal set; }
    }
}