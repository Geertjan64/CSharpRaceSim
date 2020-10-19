using Controller;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Level5Taak1
    {
        [SetUp]
        public void setup()
        {
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.PlaceParticipants();
        }
      

        [Test]
        public void TimerGetsInitializedToFiveHundred() {
            //Data.CurrentRace.Timer;   
        }
    }
}
