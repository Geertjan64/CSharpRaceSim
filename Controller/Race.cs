using Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Controller
{
    public class Race
    {

        private const int TRACK_LENGTH = 1000;

        public Track Track;
        
        private List<IParticpant> Particpants;
        
        //private DateTime StartTime;
        
        private Random _random;
        
        private Dictionary<Section, SectionData> _positions;
        
        private Timer _timer;

        public RaceData raceData;
        
        public event OnDriversChanged DriversChanged;
        public delegate void OnDriversChanged (object sender, DriversChangedEventArgs e );

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
           // Console.SetCursorPosition(0, 54);
           // Console.WriteLine("The Elapsed event was last raised at {0:HH:mm:ss.fff}", e.SignalTime);

            if( Data.CurrentRace.raceData.FinishedParticipantCount() == Data.CurrentRace.Particpants.Count)
            {
                // Race is finished 
                Data.RaceFinished();
                return;
            }


            moveParticipants();
            Data.CurrentRace.DriversChanged.Invoke(Data.CurrentRace, new DriversChangedEventArgs(Data.CurrentRace.Track));



        }

        private static void getSectionsWithParticipants ( out Stack<Section> sections)
        {
            sections = new Stack<Section>();
            foreach (Section sec in Data.CurrentRace.Track.Sections)
            {
                var section_data = Data.CurrentRace.GetSectionData(sec);
                if (section_data.Left != null || section_data.Right != null)
                {
                    sections.Push(sec);
                }
            }

        }

        private static void FinishLap(SectionData section, bool RightSide = false )
        {
            if (RightSide)
            {
                Data.CurrentRace.raceData.RondeToevoegen(section.Right);
                // if true we have completed the race 
                if (Data.CurrentRace.raceData.GetRaceRondesVoor(section.Right) == 3)
                {
                    //Console.SetCursorPosition(0, 58);
                    //Console.WriteLine($"{data.Right.Name} has finished the race!");
                    Data.CurrentRace.raceData.ParticipantFinished(section.Right);
                    section.Right = null;
                }
            }
            else
            {
                Data.CurrentRace.raceData.RondeToevoegen(section.Left);
                // if true we have completed the race 
                if (Data.CurrentRace.raceData.GetRaceRondesVoor(section.Left) == 3)
                {
                    // Console.SetCursorPosition(0, 58);
                    // Console.WriteLine($"{data.Left.Name} has finished the race!");
                    Data.CurrentRace.raceData.ParticipantFinished(section.Left);
                    section.Left = null;
                }
            }
        }

        public static void moveParticipants()
        {
            Stack<Section> SectionsWithParticipants ;
            getSectionsWithParticipants(out SectionsWithParticipants);

            while (SectionsWithParticipants.Count > 0)
            {
                // Current section with its section data. 
                Section section = SectionsWithParticipants.Pop();
                SectionData data = Data.CurrentRace.GetSectionData(section);

                // increase distance traveled foreach participant 
                if( data.Left != null )
                    data.DistanceLeft += (data.Left.Equipment as Car).getCarVelocity();
                if( data.Right != null )
                    data.DistanceRight += (data.Right.Equipment as Car).getCarVelocity();

                Track CurrentTrack = Data.CurrentRace.Track;
                Section nextSection = CurrentTrack.Sections.Find(section)?.Next?.Value;
                SectionData nextSection_data =  nextSection == null ? 
                    Data.CurrentRace.GetSectionData(CurrentTrack.Sections.First.Value) : 
                    Data.CurrentRace.GetSectionData(nextSection);
               
                if (data.DistanceLeft >= TRACK_LENGTH && data.Left != null )
                {
                    // if true we have done one lap 
                    if (nextSection == null) {
                        FinishLap(data);
                    }
                   
                    // lets move 
                    if (nextSection_data.Left == null)
                    {
                        nextSection_data.Left = data.Left;
                        data.DistanceLeft = 0;
                        data.Left = null;
                    }
                    else if( nextSection_data.Right == null)
                    {
                        nextSection_data.Right = data.Left;
                        data.DistanceLeft = 0;
                        data.Left = null;
                    }
                   
                    
                }

                if (data.DistanceRight >= TRACK_LENGTH && data.Right != null)
                {
                    // if true we have finished a lap 
                    if( nextSection == null )
                    {
                        FinishLap(data, true);
                    }
                    
                   
                    if ( nextSection_data.Right == null)
                    {
                        nextSection_data.Right = data.Right;
                        data.DistanceRight = 0;
                        data.Right = null;
                    } else if ( nextSection_data.Left == null) {
                        nextSection_data.Left = data.Right;
                        data.DistanceRight = 0;
                        data.Right = null;
                    }
                        
                    
                    
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

        public void RadomizeEquipment()
        {
            foreach(IParticpant particpant in Particpants)
            {
                particpant.Equipment.Quality = _random.Next();
                particpant.Equipment.Performance = _random.Next();
            }
        }

        public void RaceCleanup()
        {
            _timer.Elapsed -= OnTimedEvent;
            DriversChanged = null;
        }

    }
}
