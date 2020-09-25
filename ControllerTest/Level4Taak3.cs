using Controller;
using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Level4Taak3
    {
        private Competition _competition;
        private Race _race;
        [SetUp]
        public void createDummyData()
        {

            _competition = new Competition();
            _competition.tracks.Enqueue(new Track(
                    "Testgrid",
                    new SectionTypes[] {
                        SectionTypes.LeftCorner,
                        SectionTypes.StartGrid,
                        SectionTypes.StartGrid,
                        SectionTypes.StartGrid,
                        SectionTypes.Straight,
                        SectionTypes.Straight,
                        SectionTypes.LeftCorner,
                        SectionTypes.LeftCorner,
                        SectionTypes.Straight,
                        SectionTypes.Straight,
                        SectionTypes.Straight,
                        SectionTypes.Straight,
                        SectionTypes.Finish,
                        SectionTypes.LeftCorner

                    }));

            _competition.particpants.Add(new Driver());
            _competition.particpants.Add(new Driver());

            _race = new Race(_competition.NextTrack(), _competition.particpants);


        }

        [Test]
        public void Track_Shouldhave_A_Participant_At_StartGridOne()
        {
         
            _race.PlaceParticipants();
            Stack<Section> Startgrids = findStartGrids(_race.Track);

            
            SectionData data = _race.GetSectionData(Startgrids.Pop());

            Assert.IsTrue(data.Left != null || data.Right != null);
        }


        [Test]
        public void All_Drivers_Are_Added_To_A_Startgrid()
        {
            _race.PlaceParticipants();
            int driversPlaced = 0;

            Stack<Section> Startgrids = findStartGrids(_race.Track);

            while( Startgrids.Count > 0)
            {
                Section StartGrid = Startgrids.Pop();
                if (_race.GetSectionData(StartGrid).Left != null)
                    driversPlaced++;
                if (_race.GetSectionData(StartGrid).Right != null)
                    driversPlaced++;

            }
            Assert.AreEqual(driversPlaced, _competition.particpants.Count);
        }


        public Stack<Section> findStartGrids( Track track)
        {
            var result = new Stack<Section>();
            foreach (Section s in track.Sections)
            {
                if (s.SectionType == SectionTypes.StartGrid)
                {
                    result.Push(s);
                }
            }
            return result;
        }
    }
}
