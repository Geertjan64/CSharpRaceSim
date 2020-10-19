using Controller;
using Model;
using NuGet.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ControllerTest
{
    [TestFixture]
    class Level6Taak3
    {
        Driver Micheal, Patrick, Sean, Tom;

        [SetUp]
        public void setup()
        {
            Micheal = new Driver("Micheal", 0, TeamColors.Blue);
            Patrick = new Driver("Patrick", 0, TeamColors.Blue);
            Sean = new Driver("Sean", 0, TeamColors.Blue);
            Tom = new Driver("Tom", 0, TeamColors.Blue);

            Race r = new Race(null, new List<IParticpant> { Micheal, Patrick, Sean, Tom });

            Data.Initialize();
            Data.CurrentRace = r;



        }

        [TestCase]
        public void Winners_are_correctly_determined_2()
        {
            // 1. Sean
            // 2. Patrick
            // 3. Micheal
            // 4. Tom
            Data.CurrentRace.raceData.ParticipantFinished(Sean);
            Thread.Sleep(1000);
            Data.CurrentRace.raceData.ParticipantFinished(Patrick);
            Thread.Sleep(2000);
            Data.CurrentRace.raceData.ParticipantFinished(Micheal);
            Thread.Sleep(4000);
            Data.CurrentRace.raceData.ParticipantFinished(Tom);

            (Driver p1, Driver p2, Driver p3) Winners = Race.DetermineWinners();
            Assert.AreEqual(Winners.p1, Sean);
            Assert.AreEqual(Winners.p2, Patrick);
            Assert.AreEqual(Winners.p3, Micheal);

        }
        [TestCase]
        public void Winners_are_correctly_determined()
        {
            // 1. Micheal
            // 2. Sean
            // 3. Tom
            // 4. Patrick 
            Data.CurrentRace.raceData.ParticipantFinished(Micheal);
            Thread.Sleep( 1000 );
            Data.CurrentRace.raceData.ParticipantFinished(Sean);
            Thread.Sleep(2000);
            Data.CurrentRace.raceData.ParticipantFinished(Tom);
            Thread.Sleep(4000);
            Data.CurrentRace.raceData.ParticipantFinished(Patrick);

            (Driver p1, Driver p2 , Driver p3) Winners = Race.DetermineWinners();
            Assert.AreEqual(Winners.p1, Micheal);
            Assert.AreEqual(Winners.p2, Sean);
            Assert.AreEqual(Winners.p3, Tom);

        }
    }
}
