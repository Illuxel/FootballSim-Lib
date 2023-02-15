namespace FootBalLife.GameDB.Models;

internal class Director
{
    public string PersonId { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
