using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class ParticipantPoints : ParticipantStatistic
    {
        public string Name { get; set; }
        public int Points { get;  set; }

        public void Add<T>(List<T> lijst) where T  : class ,  ParticipantStatistic 
        {
            List<ParticipantPoints> l = lijst as List<ParticipantPoints>;
            if (l.Contains(this))
            {
                Points++;
            }
            else
            {
                l.Add(this);
            }
        }
    }
}
