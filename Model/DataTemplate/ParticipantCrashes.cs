using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Model
{
    public class ParticipantCrashes : ParticipantStatistic
    {
        public string Name { get; set; }
        public string TrackName { get; set; }
        public Section section { get; set; }

        public void Add<T>(List<T> lijst) where T : class, ParticipantStatistic
        {
            // do nothing for now

            ///throw new NotImplementedException();
        }

        public string FindBest<T>(List<T> lijst) where T : class, ParticipantStatistic
        {
            Dictionary<String, int> crashCount = new Dictionary<string, int>();
           foreach ( var p in lijst)
            {
                if ( p is ParticipantCrashes)
                {
                    if (crashCount.ContainsKey(p.Name))
                    {
                        crashCount[p.Name]++;

                    }
                    else
                    {
                        crashCount.Add(p.Name, 1);
                    }
                }
            }


            (string naam, int crashes) lowest = ("Onbekend", 999999);
            foreach ( KeyValuePair<string,int>kvp in crashCount)
            {
                if ( kvp.Value < lowest.crashes)
                {
                    lowest.naam = kvp.Key;
                    lowest.crashes = kvp.Value;
                }
            }

            return lowest.naam;
        }
    }
}
