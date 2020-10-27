using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticpant> particpants { get; set; }
        public Queue<Track> tracks { get; set; }

        public CompetitionData<ParticipantPoints> CompetitionPoints {get ; set;} 
        public CompetitionData<ParticpantSectionTime> CompetitionSectionTimes { get; set; }

        public CompetitionData<ParticipantLapTime> CompetitionLapTimes { get; set; }
        public CompetitionData<ParticipantCrashes> CompetitionCrashes { get; set; }

        public Competition() {
            particpants = new List<IParticpant>();
            tracks = new Queue<Track>();
            CompetitionPoints = new CompetitionData<ParticipantPoints>();
            CompetitionSectionTimes = new CompetitionData<ParticpantSectionTime>();
            CompetitionLapTimes = new CompetitionData<ParticipantLapTime>();
            CompetitionCrashes = new CompetitionData<ParticipantCrashes>();
        }

        public void distributePoints ((Driver p1, Driver p2, Driver p3) Winners)
        {
            CompetitionPoints.AddData(new ParticipantPoints() { Points = 5, Name = Winners.p1.Name } );
            CompetitionPoints.AddData(new ParticipantPoints() { Points = 3, Name = Winners.p2.Name } );
            CompetitionPoints.AddData(new ParticipantPoints() { Points = 1, Name = Winners.p3.Name } );

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
