namespace DatabaseLayer.Services
{
    public class NewPositionCreator
    {
        public Position Create(string positionCode, PlayerFieldPartPosition playerPosition)
        {
            return new Position()
            {
                Code = positionCode,
                Location = playerPosition                
            };
        }
    }
}
