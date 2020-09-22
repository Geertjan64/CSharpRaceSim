using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Model_Competition_NextTrackShould
    {
        private static Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
            
        }
        [Test]
        public void NextTRack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);

        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var track = new Track("Mijn track", new SectionTypes[1]);
            _competition.tracks.Enqueue(track);

            Track result = _competition.NextTrack();

            Assert.AreEqual(track, result);
        }

    }
}
