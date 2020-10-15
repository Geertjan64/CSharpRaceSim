using Model;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Controller
{
    public class Race
    {

        // Track van het type Track, Participants van het type List<IParticipant> en StartTime van het type DateTime
        private const int TRACK_LENGTH = 5000;

        public Track Track;
        
        private List<IParticpant> Particpants;
        
        private DateTime StartTime;
        
        private Random _random;
        
        private Dictionary<Section, SectionData> _positions;
        
        private Timer _timer;

        public RaceData raceData;
        
        
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
            raceData = new RaceData();
            _timer.Elapsed += OnTimedEvent;
        }

        public  void Start()
        {
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Show time
           // Console.SetCursorPosition(0, 25);
           // Console.WriteLine("The Elapsed event was last raised at {0:HH:mm:ss.fff}", e.SignalTime);

            moveParticipants();
            Data.CurrentRace.DriversChanged.Invoke(Data.CurrentRace, new DriversChangedEventArgs(Data.CurrentRace.Track));

        }

        public static void moveParticipants()
        {
            // IMPORTANT: the leading participant in the race should be moved first.
            // step 1 figure out where all participants are on the track
            // step 2 check if they have been on the track long enough to go to the next 
            // step 3 check if the next track has an empty spot.
            // step 4 if not it stay where it is , if there is move to that spot
            // step 5 render the tracks that have changed 

            var sections_with_player = new Stack<Section>();
            var data_lib = new Dictionary<Section, SectionData>();

            foreach (Section sec in Data.CurrentRace.Track.Sections)
            {
                SectionData data = Data.CurrentRace.GetSectionData(sec);
                if (data.Left != null || data.Right != null)
                {
                    sections_with_player.Push(sec);
                    data_lib.Add(sec, data);
                }
            }

            while (sections_with_player.Count > 0)
            {
                // Current section with its section data. 
                Section section = sections_with_player.Pop();
                SectionData data = data_lib[section];

                // 
                if( data.Left != null )
                    data.DistanceLeft += (data.Left.Equipment as Car).getCarVelocity();
                if( data.Right != null )
                    data.DistanceRight += (data.Right.Equipment as Car).getCarVelocity();
                // 
                Track CurrentTrack = Data.CurrentRace.Track;

                // next section with its section data.
                Section nextSection = CurrentTrack.Sections.Find(section)?.Next?.Value;
                // Might indicicate we were on the last section 
                SectionData nextSection_data;
                if (nextSection == null)
                {
                    // we finished a round on the circuit 
                    nextSection_data = Data.CurrentRace.GetSectionData(CurrentTrack.Sections.First.Value);

                    if (data.Left != null )
                    {
                        Data.CurrentRace.raceData.RondeToevoegen(data.Left);
                    }
                    if(data.Right != null )
                    {
                        Data.CurrentRace.raceData.RondeToevoegen(data.Right);
                    }
                }
                else
                {
                    nextSection_data = Data.CurrentRace.GetSectionData(nextSection);
                }
                
                

                if (data.DistanceLeft >= TRACK_LENGTH && nextSection_data.Left == null)
                {
                    // lets move left 
                    nextSection_data.Left = data.Left;
                    data.DistanceLeft = 0;
                    data.Left = null;
                }

                if (data.DistanceRight >= TRACK_LENGTH && nextSection_data.Right == null)
                {
                    nextSection_data.Right = data.Right;
                    data.DistanceRight = 0;
                    data.Right = null;
                }

               

            }

            Data.CurrentRace.DriversChanged.Invoke(Data.CurrentRace, new DriversChangedEventArgs(Data.CurrentRace.Track));
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
