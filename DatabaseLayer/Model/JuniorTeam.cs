namespace DatabaseLayer.Model
{
    public class JuniorTeam
    {
        public string TeamId { get; set; }
        public int? AverageTeamRating { get; set; }
        public JuniorPlayerPreview? BestJunior { get; set; }
    }
}
