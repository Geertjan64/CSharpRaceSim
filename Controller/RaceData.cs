using Model;
using System.Collections.Generic;

namespace Controller
{
    public struct RaceData
    {
        private  Dictionary < IParticpant , uint > _raceRondes { get; set; }


        public void RondeToevoegen(IParticpant participant)
        {
            if (_raceRondes == null)
                _raceRondes = new Dictionary<IParticpant, uint>();

            if (_raceRondes.ContainsKey(participant))
            {
                _raceRondes[participant]++;
            }
            else
            {
                _raceRondes.Add(participant, 1);
            }

        }

        public delegate void onForeach( IParticpant key ,  uint value );
        public  void ForeachParticipantInRondes (  onForeach DoFunction)
        {
            if (_raceRondes == null)
                _raceRondes = new Dictionary<IParticpant, uint>();

            foreach ( KeyValuePair<IParticpant , uint > keyValue in _raceRondes )
            {
                DoFunction(keyValue.Key, keyValue.Value);
            }
        }

        public uint GetRaceRondesVoor(IParticpant particpant)
        {
            return _raceRondes[particpant];
        }
    }
}
