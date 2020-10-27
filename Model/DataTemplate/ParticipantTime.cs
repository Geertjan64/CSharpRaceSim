using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ParticipantTime : ParticipantStatistic
    {
        public string Name { get; set; }
        public  TimeSpan time { get; set; }

        public void Add<T>(List<T> lijst) where T : class, ParticipantStatistic
        {
           // do nothing  for now 
           // lijst.Add( this);
        }
    }
}
