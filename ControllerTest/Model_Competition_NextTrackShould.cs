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
        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            var track = new Track("TestTrack", new SectionTypes[0]);
            _competition.tracks.Enqueue(track);
            Track result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            var track1 = new Track("TestTrack1", new SectionTypes[0]);
            var track2 = new Track("TestTrack2", new SectionTypes[0]);

            _competition.tracks.Enqueue(track1);
            _competition.tracks.Enqueue(track2);

            Track result = _competition.NextTrack();
            Assert.AreEqual(track1, result);

            result = _competition.NextTrack();

            Assert.AreEqual(track2, result);

        }

    }
}
