using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticpant> particpants { get; set; }
        public Queue<Track> tracks{ get; set; }

        public Track NextTrack()
        {
            return null;
        }
    }
}
