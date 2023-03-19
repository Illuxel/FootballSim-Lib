namespace FootBalLife.Database
{
    public class Player
    {
        public string PersonID { get; set; }

        public string? PositionCode { get; set; }

        public string? ContractID { get; set; }

        public int Kick { get; set; }

        //відповідає за рівень "свіжості гравця"
        public long Endurance { get; set; }

        //ігрові характеристики
        public long Strike { get; set; }
        public long Speed { get; set; }
        //у воротаря ця властивість - Вибір позиції
        public long Physics { get; set; }
        public long Technique { get; set; }
        public long Passing { get; set; }

        //у воротаря ця властивість - Рефлекси
        public int Dribbling { get; set; }

        public Person? Person { get; internal set; }
        public Position? Position { get; set; }
    }
}