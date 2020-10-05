using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Controller
{
    public class Race
    {
        // Track van het type Track, Participants van het type List<IParticipant> en StartTime van het type DateTime
        public Track Track;
        private List<IParticpant> Particpants;
        private DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        public event OnDriversChanged DriversChanged;
        public delegate void OnDriversChanged (object sender, DriversChangedEventArgs e );

        // Deze constructor heeft als parameters: Track en List<IParticipant>. Gebruik de parameters om de waarden van de properties Track en Participants te zetten.
        public Race(Track track, List<IParticpant> particpants)
        {
            Track = track;
            Particpants = particpants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
        }

        public  void Start()
        {
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.SetCursorPosition(0, 25);
            Console.WriteLine("The Elapsed event was last raised at {0:HH:mm:ss.fff}", e.SignalTime);
        }
        public void PlaceParticipants()
        {
            Stack<Section> StartingGrid = new Stack<Section>();
            
            foreach(Section section in Track.Sections)
            {
                if( section.SectionType == SectionTypes.StartGrid)
                {
                    StartingGrid.Push(section);
                }
            }


            int Participantcount = 0;
            while ( StartingGrid.Count > 0 )
            {
                Section s = StartingGrid.Pop();
                SectionData data = GetSectionData(s);

                if (Particpants.Count - Participantcount > 0)
                {
                    data.Left = Particpants[Participantcount];
                    Participantcount++;
                }
                if (Particpants.Count - Participantcount > 0)
                {
                    data.Right = Particpants[Participantcount];
                    Participantcount++;
                }
                
            }

        }


        public SectionData GetSectionData(Section section)
        {
            if (_positions.ContainsKey(section) == false) { 
            
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
