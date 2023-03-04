using System.Collections.Generic;

namespace FootBalLife.Database
{
    public class Position
    {
        public string Id { get; internal set; }
        public PlayerPosition Location { get; set; }
        public string? Name { get; set; }

        public ICollection<Player> Players { get; internal set; } = new List<Player>();
    }
}