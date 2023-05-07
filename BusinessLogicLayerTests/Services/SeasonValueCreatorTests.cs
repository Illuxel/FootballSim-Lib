using BusinessLogicLayer.Services;
using NUnit.Framework;

namespace BusinessLogicLayerTests.Services
{
    public class SeasonValueCreatorTests
    {
        [Test]
        public void GetSeason_2023()
        {
            //arrange
            var expectedSeason = "2023/2024";

            //act
            var seasonValueCreator = new SeasonValueCreator();
            var result = seasonValueCreator.GetSeason(2023);
            //assert
            Assert.AreEqual(expectedSeason, result);
        }

        [Test]
        public void GetNextSeason_2023_Add2years()
        {
            //arrange
            var currentSeason = "2023/2024";
            var addedSeason = 2;
                
            var expectedResultSeason = "2025/2026";
                
            //act
            var seasonValueCreator = new SeasonValueCreator();
            var result = seasonValueCreator.GetFutureSeason(currentSeason, addedSeason);
                
            //assert
            Assert.AreEqual(expectedResultSeason, result);
        }
    }
    
}
