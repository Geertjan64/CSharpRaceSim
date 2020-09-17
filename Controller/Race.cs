using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller
{
    class Race
    {
        // Track van het type Track, Participants van het type List<IParticipant> en StartTime van het type DateTime
        private Track Track;
        private List<IParticpant> Particpants;
        private DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        // Deze constructor heeft als parameters: Track en List<IParticipant>. Gebruik de parameters om de waarden van de properties Track en Participants te zetten.
        public Race(Track track, List<IParticpant> particpants)
        {
            this.Track = track;
            this.Particpants = particpants;
            this._random = new Random(DateTime.Now.Millisecond);

        }

        public SectionData GetSectionData(Section section)
        {
            if (_positions[section] == null)
            {
                _positions[section] = new SectionData();
            }

            return _positions[section];
        }

        //Breid de klasse Race uit met een methode RandomizeEquipment.
        //Itereer over alle deelnemers in de competitie.Geef de properties Quality en Performance van de property Equipment een willekeurige waarde.
        public void RadomizeEquipment()
        {
            foreach(IParticpant particpant in Particpants)
            {
                particpant.Equipment.Quality = _random.Next();
                particpant.Equipment.Performance = _random.Next();
            }
        }


    }
}
