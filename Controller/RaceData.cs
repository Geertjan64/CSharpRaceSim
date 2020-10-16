using Model;
using System;
using System.Collections.Generic;

namespace Controller
{
    public struct RaceData
    {
        private  Dictionary < IParticpant , uint > _raceRondes { get; set; }
        private List<IParticpant> FinishedParticipants;

        public void ParticipantFinished(IParticpant particpant)
        {
            if (FinishedParticipants == null)
            {
                FinishedParticipants = new List<IParticpant>();
            }

            if(FinishedParticipants.Contains(particpant) == false)
            {
                FinishedParticipants.Add(particpant);
            }

        }

        public int FinishedParticipantCount()
        {
            if( FinishedParticipants == null)
            {
                FinishedParticipants = new List<IParticpant>();
            }

            return FinishedParticipants.Count;
        } 

        public void RondeToevoegen(IParticpant participant)
        {
            if (_raceRondes == null)
            {
                _raceRondes = new Dictionary<IParticpant, uint>();
            }
            if (participant == null)
            {
                throw new Exception("participant given is null");

            }

            if (_raceRondes.ContainsKey(participant))
            {
                _raceRondes[participant]++;
            } else
            {
                _raceRondes.Add(participant, 1);
            }

           // Console.SetCursorPosition(100,30);
           // Console.WriteLine($"{participant.Name} has started lap {_raceRondes[participant]}");

        }

        public uint GetRaceRondesVoor(IParticpant particpant)
        {
            if(_raceRondes.ContainsKey(particpant))
            {
                return _raceRondes[particpant];

            }
            else
            {
                return 0;
            }
        }


        public delegate void onForeach(IParticpant key, uint value);
        public void ForeachParticipantInRondes(onForeach DoFunction)
        {
            if (_raceRondes == null)
                _raceRondes = new Dictionary<IParticpant, uint>();

            foreach (KeyValuePair<IParticpant, uint> keyValue in _raceRondes)
            {
                DoFunction(keyValue.Key, keyValue.Value);
            }
        }

    }
}
