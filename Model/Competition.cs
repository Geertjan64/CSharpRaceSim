using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticpant> particpants { get; set; }
        public Queue<Track> tracks{ get; set; }


        public Competition() {
            particpants = new List<IParticpant>();
            tracks = new Queue<Track>();
        }

        public Track NextTrack()
        {
            try
            {
                return tracks.Dequeue();
            } catch (InvalidOperationException e)
            {
                return null;
            }
            
        }
    }
}
