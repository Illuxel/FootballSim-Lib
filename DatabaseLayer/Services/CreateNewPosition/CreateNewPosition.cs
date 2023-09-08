namespace DatabaseLayer.Services
{
    public class CreateNewPosition
    {
        public Position Create(string positionCode,string positionName, PlayerFieldPartPosition playerPosition)
        {
            return new Position()
            {
                Code = positionCode,
                Name = positionName,
                Location = playerPosition                
            };
        }
    }
}
