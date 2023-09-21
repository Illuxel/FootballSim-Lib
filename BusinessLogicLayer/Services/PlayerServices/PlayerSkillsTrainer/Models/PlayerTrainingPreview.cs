using DatabaseLayer;

namespace BusinessLogicLayer.Services.PlayerServices
{
    internal class PlayerTrainingPreview : IPlayerTrainingPreview
    {
        public string PersonID { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }

        public int Overall { get; set; }

        public int CurrentEndurance { get; set; }

        public int AfterTrainEndurance { get; set; }
    }
}
