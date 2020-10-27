using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.SetCursorPosition(70,0 );
            Console.WriteLine("The Elapsed event was last raised at {0:HH:mm:ss.fff}", e.SignalTime);

            if(isRaceFinished())
            {
                // Race is finished 
                (Driver p1, Driver p2, Driver p3) Winners = DetermineWinners();

                // Show Winners 
                const int LEFTPADDING = 50;
                Console.SetCursorPosition(LEFTPADDING, 6);
                Console.Write("♫DING DONG♫");
                Console.SetCursorPosition(LEFTPADDING, 7);
                Console.WriteLine("The race results are in!"); 
                Console.SetCursorPosition(LEFTPADDING, 8);
                Console.Write($"① {Winners.p1.Name} | Receiving 5 Points");
                Console.SetCursorPosition(LEFTPADDING, 9);
                Console.Write($"② {Winners.p2.Name} | Receiving 3 points");
                Console.SetCursorPosition(LEFTPADDING, 10);
                Console.Write($"③ {Winners.p3.Name} | Receiving 1 point");
                Console.SetCursorPosition(LEFTPADDING, 11);
                Console.Write("Press any  key to continue");
                Console.ReadKey();

                Data.Competition.distributePoints(Winners);

                Data.RaceFinished();
                return;
            }

            RandomBreakdown();
            
            moveParticipants();
            Data.CurrentRace.DriversChanged.Invoke(Data.CurrentRace, new DriversChangedEventArgs(Data.CurrentRace.Track));

        }

        public static (Driver first, Driver second, Driver third) DetermineWinners()
        {
            Driver first = null;
            Driver second = null;
            Driver third = null;

            DateTime finishTime_first = DateTime.Now;
            DateTime finishTime_second = DateTime.Now;
            DateTime finishTime_third = DateTime.Now;


            
            foreach(Driver d in Data.CurrentRace.Particpants)
            {
                DateTime finishTime = Data.CurrentRace.raceData.getFinishTime(d);
                if ( finishTime < finishTime_first)
                {
                    third = second;
                    second = first;
                    first = d;
                    finishTime_third = finishTime_second;
                    finishTime_second = finishTime_first;
                    finishTime_first = finishTime;
                    
                } 
                else if ( finishTime > finishTime_first && finishTime < finishTime_second )
                {
                    third = second;
                    second = d;
                    finishTime_third = finishTime_second;
                    finishTime_second = finishTime;
                    
                } 
                else if ( finishTime > finishTime_second && finishTime < finishTime_third )
                {
                    third = d;
                    finishTime_third = finishTime;
                    
                }
            }

            return (first, second, third);
        }

        private static void RandomBreakdown() 
        {
            var _random = new Random();
            int line = 2;
            foreach ( Driver d  in Data.CurrentRace.Particpants)
            {
                // Don't break if driver has finished race
                if (Data.CurrentRace.raceData.GetRaceRondesVoor(d as IParticpant) == 3)
                    continue;

                if (d.Equipment.IsBroken)
                {
                    // have we repaired 
                    int base_chance_of_repair = 60;//80;
                    int real_chance = base_chance_of_repair - (~d.Equipment.Quality);
                    if( _random.Next(0, 100) < real_chance)
                    {
                        d.Equipment.IsBroken = false;
                        Console.SetCursorPosition(70, line++);
                        Console.WriteLine($"{d.Name}'s car has been repaired!");
                    }
                    continue;
                }
                int salt = (_random.Next() % 10)+1;
                int chance =  100 * d.Equipment.Quality / salt ;
                d.Equipment.IsBroken = (chance/ 4) > _random.Next(0, 1000);
                if(d.Equipment.IsBroken)
                {

                    Data.Competition.CompetitionCrashes.AddData(new ParticipantCrashes()
                    {
                        Name = d.Name,
                        TrackName = Data.CurrentRace.Track.Name,
                        section = getSectionWithParticipant(d as IParticpant)
                    });

                    // Modify cars properties 
                    int what_should_change = _random.Next(0, 6);

                    switch (what_should_change)
                    {
                        case (1):
                        case (3):
                            // incapacitate Quality 
                            d.Equipment.Quality -= 1;
                            break;
                        case (4):
                        case (2):
                            // incapacitate Performance
                            d.Equipment.Quality -= 1;
                            break;
                        case (5):
                            // incapacitate Speed and Quality
                            d.Equipment.Speed -= 1;
                            d.Equipment.Quality -= 1;
                            break;
                        case (6):
                            // incapacitate Performance and Quality
                            d.Equipment.Performance -= 1;
                            d.Equipment.Quality -= 1;
                            break;
                    }
                    Console.SetCursorPosition(70, line++);
                    Console.WriteLine($"{d.Name}'s car has broken down!" );

                }
            }    
        }

        private static Section getSectionWithParticipant(IParticpant p)
        {            
            foreach ( Section s in Data.CurrentRace.Track.Sections)
            {
                var sd = Data.CurrentRace.GetSectionData(s);
                if (sd.Left == p || sd.Right == p)
                    return s;

            }
            return null;
        }

        private static bool isRaceFinished()
        {
            return Data.CurrentRace.raceData.FinishedParticipantCount() == Data.CurrentRace.Particpants.Count;
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

                Data.Competition.CompetitionLapTimes.AddData(new ParticipantLapTime()
                {
                    Name = section.Right.Name,
                    track = Data.CurrentRace.Track,
                    time = Data.CurrentRace.raceData.getLapStartTime(section.Right) - DateTime.Now
                });

                Data.CurrentRace.raceData.AddLapStartTime(section.Right);

                // if true we have completed the race 
                if (Data.CurrentRace.raceData.GetRaceRondesVoor(section.Right) == 3)
                {
                    //Console.SetCursorPosition(0, 58);
                    //Console.WriteLine($"{data.Right.Name} has finished the race!");
                    Data.CurrentRace.raceData.ParticipantFinished(section.Right);
                    Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                    {
                        Name = section.Right.Name,
                        section = Data.CurrentRace.Track.Sections.Last.Value,
                        time = section.RightEnterTime - DateTime.Now
                    }) ;
                    section.Right = null;
                    section.DistanceRight = 0;
                    
                }
                
            }
            else
            {
                Data.CurrentRace.raceData.RondeToevoegen(section.Left);
                Data.Competition.CompetitionLapTimes.AddData(new ParticipantLapTime()
                {
                    Name = section.Left.Name,
                    track = Data.CurrentRace.Track,
                    time = Data.CurrentRace.raceData.getLapStartTime(section.Left) - DateTime.Now
                });
                Data.CurrentRace.raceData.AddLapStartTime(section.Left);

                // if true we have completed the race 
                if (Data.CurrentRace.raceData.GetRaceRondesVoor(section.Left) == 3)
                {
                    // Console.SetCursorPosition(0, 58);
                    // Console.WriteLine($"{data.Left.Name} has finished the race!");
                    Data.CurrentRace.raceData.ParticipantFinished(section.Left);
                    Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                    {
                        Name = section.Left.Name,
                        section = Data.CurrentRace.Track.Sections.Last.Value,
                        time = section.LeftEnterTime - DateTime.Now
                    });
                    section.Left = null;
                    section.DistanceLeft = 0;
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
                if( data.Left != null && data.Left.Equipment.IsBroken == false)
                    data.DistanceLeft += (data.Left.Equipment as Car).getCarVelocity();
                if( data.Right != null && data.Right.Equipment.IsBroken == false)
                    data.DistanceRight += (data.Right.Equipment as Car).getCarVelocity();

                Track CurrentTrack = Data.CurrentRace.Track;
                Section nextSection = CurrentTrack.Sections.Find(section)?.Next?.Value;
                SectionData nextSection_data =  nextSection == null ? 
                    Data.CurrentRace.GetSectionData(CurrentTrack.Sections.First.Value) : 
                    Data.CurrentRace.GetSectionData(nextSection);
               
                if (data.DistanceLeft >= TRACK_LENGTH && data.Left != null && data.Left.Equipment.IsBroken == false)
                {
                    // if true we have done one lap 
                    if (nextSection == null) {
                        FinishLap(data);
                    }
                    else
                    {
                        // lets move 
                        if (nextSection_data.Left == null)
                        {
                            Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                            {
                                Name = data.Left.Name,
                                section = section,
                                time = data.LeftEnterTime - DateTime.Now
                            });
                            nextSection_data.Left = data.Left;
                            nextSection_data.LeftEnterTime = DateTime.Now;
                            data.DistanceLeft = 0;
                            data.Left = null;
                        }
                        else if (nextSection_data.Right == null)
                        {
                            Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                            {
                                Name = data.Left.Name,
                                section = section,
                                time = data.LeftEnterTime - DateTime.Now
                            });
                            nextSection_data.RightEnterTime = DateTime.Now;
                            nextSection_data.Right = data.Left;
                            data.DistanceLeft = 0;
                            data.Left = null;
                        }
                    }
                   
                   
                   
                    
                }

                if (data.DistanceRight >= TRACK_LENGTH && data.Right != null && data.Right.Equipment.IsBroken == false)
                {
                    // if true we have finished a lap 
                    if( nextSection == null )
                    {
                        FinishLap(data, true);
                    }
                    else
                    {
                        if (nextSection_data.Right == null)
                        {
                            Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                            {
                                Name = data.Right.Name,
                                section = section,
                                time = data.RightEnterTime - DateTime.Now
                            });
                            nextSection_data.RightEnterTime = DateTime.Now;
                            nextSection_data.Right = data.Right;
                            data.DistanceRight = 0;
                            data.Right = null;
                        }

                        else if (nextSection_data.Left == null)
                        {
                            Data.Competition.CompetitionSectionTimes.AddData(new ParticpantSectionTime()
                            {
                                Name = data.Right.Name,
                                section = section,
                                time = data.RightEnterTime - DateTime.Now
                            });
                            nextSection_data.LeftEnterTime = DateTime.Now;
                            nextSection_data.Left = data.Right;
                            data.DistanceRight = 0;
                            data.Right = null;
                        }
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
                    Data.CurrentRace.raceData.AddLapStartTime(Particpants[Participantcount]);
                    data.Left = Particpants[Participantcount];
                    data.LeftEnterTime = DateTime.Now;
                    Participantcount++;
                }
                if (Particpants.Count - Participantcount > 0)
                {
                    Data.CurrentRace.raceData.AddLapStartTime(Particpants[Participantcount]);
                    data.Right = Particpants[Participantcount];
                    data.RightEnterTime = DateTime.Now;
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
