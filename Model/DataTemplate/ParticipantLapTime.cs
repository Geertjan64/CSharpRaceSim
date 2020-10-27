using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ParticipantLapTime : ParticipantTime
    {
        public Track track { get; set; }
    }
}
