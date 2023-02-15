namespace FootBalLife.GameDB.Models;

internal class Role
{
    public long Id { get; set; }

    public long? IsNpc { get; set; }

    public byte[]? Icon { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
