using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Model_Track_Should
    {
        private static Track _track { get; set; }
        [SetUp]
        public void setup_test()
        {
            _track = new Track("TestTrack", new SectionTypes[0]);
        }
        [Test]
        public void Track_ShouldTake_TwoParameters()
        {
            var param1 = "TestTrack1";
            var param2 = new SectionTypes[0];
            Track track = new Track(param1,param2 );
            Assert.IsNotNull(track);
            Assert.AreEqual(track.Name, "TestTrack1");
            Assert.IsTrue(track.Sections.Count == param2.Length);
        }

        [Test]
        public void Track_SectionTypeArrayToList_ShouldReturn_List()
        {
            SectionTypes[] sectionTypesArray = new SectionTypes[2] { SectionTypes.StartGrid, SectionTypes.Straight };
            LinkedList<Section> result = _track.SectionTypeArrayToList(sectionTypesArray);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);

        }
    }
}
