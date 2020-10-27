using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ParticipantCrashes : ParticipantStatistic
    {
        public string Name { get; set; }
        public string TrackName { get; set; }
        public Section section { get; set; }

        public void Add<T>(List<T> lijst) where T : class, ParticipantStatistic
        {
            // do nothing for now

            ///throw new NotImplementedException();
        }

    }
}
