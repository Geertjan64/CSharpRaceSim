using System.Collections.Generic;
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
            AddParticipants( 
                new List<IParticpant>() 
                { 
                    new Driver("Micheal", 0, TeamColors.Blue),
                    new Driver("Patrick", 0, TeamColors.Green),
                    new Driver("Daniel", 0, TeamColors.Red)
                });
            AddTrack();
            AddTrack();
            AddTrack();

        }

        public static void AddParticipant()
        {
            // TODO: Generate Random name 
            Data.Competition.particpants.Add(new Driver("Micheal", 0,  TeamColors.Blue));
        }
        
        public static void AddParticipants(IEnumerable<IParticpant> particpants)
        {
            foreach ( IParticpant particpant in particpants)
            {
                Data.Competition.particpants.Add(particpant);
            }
        }

        public static void AddTrack()
        {
            Data.Competition.tracks.Enqueue(
                new Track($"T{Data.Competition.tracks.Count + 1 }", 
                new SectionTypes[22] {
                    SectionTypes.LeftCorner,

                    SectionTypes.Finish,
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
                    SectionTypes.Straight,


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
                CurrentRace = new Race(t, Data.Competition.particpants);
            }
        }
    }
}
