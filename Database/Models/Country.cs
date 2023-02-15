namespace FootBalLife.GameDB.Models;

internal class Country
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public byte[]? Icon { get; set; }

    public virtual ICollection<League> Leagues { get; } = new List<League>();

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
