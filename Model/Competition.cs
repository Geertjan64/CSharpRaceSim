using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticpant> particpants { get; set; }
        public Queue<Track> tracks { get; set; }

        public CompetitionData<ParticipantPoints> CompetitionPoints {get ; set;} 

        public Competition() {
            particpants = new List<IParticpant>();
            tracks = new Queue<Track>();
            CompetitionPoints = new CompetitionData<ParticipantPoints>();
        }

        public void distributePoints ((Driver p1, Driver p2, Driver p3) Winners)
        {
            CompetitionPoints.AddData(new ParticipantPoints() { points = 5, naam = Winners.p1.Name } );
            CompetitionPoints.AddData(new ParticipantPoints() { points = 3, naam = Winners.p2.Name } );
            CompetitionPoints.AddData(new ParticipantPoints() { points = 1, naam = Winners.p3.Name } );

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
