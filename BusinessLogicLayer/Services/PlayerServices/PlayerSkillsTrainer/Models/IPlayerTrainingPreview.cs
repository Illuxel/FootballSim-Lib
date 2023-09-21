using DatabaseLayer;

namespace BusinessLogicLayer.Services.PlayerServices
{
    public interface IPlayerTrainingPreview
    {
        public string PersonID { get; }
        public string Name { get;}
        public Position Position { get; }
        public int Overall { get; }
        public int CurrentEndurance { get; }
        public int AfterTrainEndurance { get; }
    }
}
