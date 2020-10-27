using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ParticipantTime : ParticipantStatistic
    {
        public string Name { get; set; }
        public  TimeSpan time { get; set; }
    }
}
