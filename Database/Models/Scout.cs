namespace FootBalLife.GameDB.Models;

internal class Scout
{
    public string PersonId { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
