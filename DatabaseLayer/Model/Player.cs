namespace DatabaseLayer
{
    public class Player
    {
        public string PersonID { get; set; }

        public string PositionCode { get; set; }

        public string ContractID { get; set; }

        public int Kick { get; set; }
        public int Rating { get; set; }

        //відповідає за рівень "свіжості гравця"
        public int Endurance { get; set; }

        //ігрові характеристики
        public int Strike { get; set; }
        public int Speed { get; set; }
        //у воротаря ця властивість - Вибір позиції
        public int Physics { get; set; }
        public int Defending { get; set; }
        public int Passing { get; set; }

        //у воротаря ця властивість - Рефлекси
        public int Dribbling { get; set; }

        public Person Person { get; internal set; }
        public Position Position { get; set; }
    }
}