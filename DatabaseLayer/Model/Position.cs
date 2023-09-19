using System.Collections.Generic;

namespace DatabaseLayer
{
    public class Position
    {
        public string Code { get; internal set; }
        public PlayerFieldPartPosition Location { get; set; }
        public string Name { get; set; }
        public ICollection<Player> Players { get; internal set; } 

        public Position()
        {
            Players = new List<Player>();
        }
    }
}