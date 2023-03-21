namespace DatabaseLayer
{
    public class Director
    {
        public string? PersonID { get; internal set; }
        public virtual Person? Person { get; internal set; }
    }
}