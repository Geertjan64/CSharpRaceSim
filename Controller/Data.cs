using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    static class Data
    {
        public static Competition Competition { get; set; }


        public static void Initialize()
        {
            Data.Competition = new Competition();
            Data.AddParticipant();
        }


        public static void AddParticipant()
        {
            Data.Competition.particpants.Add(new Driver());
        }

        public static void AddTrack()
        {
            Data.Competition.tracks.Enqueue(new Track($"T{Data.Competition.tracks.Count + 1 }", new SectionTypes[3] )); 
        }
    }
}
