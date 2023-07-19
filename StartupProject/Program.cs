using BusinessLogicLayer.Services;

namespace StartupProject
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var serv = new JuniorPersonGeneration();
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
            serv.GenerateNewJuniorPerson(new System.DateTime(2000, 1, 1), 1, null, "Surname");
        }
    }
}
