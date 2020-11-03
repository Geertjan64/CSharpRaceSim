using System;
using System.Collections.Generic;
using System.Globalization;
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

        public string FindBest<T>(List<T> lijst) where T : class, ParticipantStatistic
        {
            (string naam , int points) mostPoints = ("Onbekend", 0);
            foreach ( var p in lijst)
            {
                if (p is ParticipantPoints)
                {
                    if ( (p as ParticipantPoints).Points > mostPoints.points)
                    {
                        mostPoints.points = (p as ParticipantPoints).Points;
                        mostPoints.naam = (p as ParticipantPoints).Name;
                    }
                }
            }

            return mostPoints.naam;
        }
    }
}
