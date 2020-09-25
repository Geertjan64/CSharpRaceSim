using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipant();
            
            AddTrack();
        }

        public static void AddParticipant()
        {
            Data.Competition.particpants.Add(new Driver());
        }
        
        public static void AddTrack()
        {
            Data.Competition.tracks.Enqueue(
                new Track($"T{Data.Competition.tracks.Count + 1 }", 
                new SectionTypes[22] {
                    SectionTypes.LeftCorner,

                    SectionTypes.Straight,
                    SectionTypes.StartGrid,
                    SectionTypes.StartGrid,
                    SectionTypes.StartGrid,
                    SectionTypes.StartGrid,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    
                    SectionTypes.LeftCorner,

                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    
                    SectionTypes.LeftCorner,

                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Finish,
                    SectionTypes.Straight,
                    
                    SectionTypes.LeftCorner,
                    
                    SectionTypes.Straight,
                    SectionTypes.Straight
                    } 
                )); 
        }

        public static void NextRace()
        {
            Track t = Competition.NextTrack();
            if (t != null)
            {
                CurrentRace = new Race(t, new List<IParticpant>());
            }
        }
    }
}
