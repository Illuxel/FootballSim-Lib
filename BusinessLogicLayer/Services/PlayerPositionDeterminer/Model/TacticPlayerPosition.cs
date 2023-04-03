using DatabaseLayer;

namespace BusinessLogicLayer.Services
{
    public class TacticPlayerPosition
    {
        public int IndexPosition { get; set; }
        public string RealPosition { get; set; }
        public Player CurrentPlayer { get; set; }
    }
}
