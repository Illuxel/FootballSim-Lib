﻿namespace DatabaseLayer
{
    public class TacticPlayerPosition
    {
        public int IndexPosition { get; set; }
        public string RealPosition { get; set; }
        public Player CurrentPlayer { get; set; }
        public PlayerFieldPartPosition FieldPosition { get; set; }
    }
}
