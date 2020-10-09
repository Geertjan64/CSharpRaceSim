using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{

    public static class Visualize
    {
        private enum Heading
        {
            Nord,
            East,
            South,
            West

        }

        private static List<DrawableSection> sectionsToRender;
        private static (int x, int y) cursor_start_position;
        private static (Heading last, Heading current) MyHeadings;
        
        private static int ParticipantPlaceholder;
        
        private delegate String[] chooseArtHeading();
        
        public static void Initialize()
        {
            Console.CursorVisible = false;

            resetRenderer();
          
            Data.CurrentRace.DriversChanged += OnDriversChanged;

            Console.WriteLine($"Race has {Data.Competition.particpants.Count} participants");
            Console.WriteLine($"Racing on Track {Data.CurrentRace.Track.Name}");
            visualize_carStats_per_driver();
        }

        private static void visualize_carStats_per_driver()
        {

            // Show stats
            int x = Console.BufferWidth - 10;
            int y = 25;
            foreach (Driver d in Data.Competition.particpants)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine($"\nCar stats ({d.Name}):");
                Console.WriteLine($"Performance: { d.Equipment.Performance}");
                Console.WriteLine($"Quality: { d.Equipment.Quality}");
                Console.WriteLine($"Speed: { d.Equipment.Speed}");
                Console.WriteLine($"Car Velocity : { ((Car)d.Equipment).getCarVelocity()}");
              
                y += 6;
            }
        }

        public static void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            Console_Extension.DrawText((0, 50), $"OnDriversChanged in Visualize! { DateTime.Now}");
            DrawTrack(args.track);
        }

        private static void placePlaceholderParticipantsOnTrack()
        {

            // place placholders for participants on track 
            Stack<DrawableSection> StartGrids;
            GetStartGrids(out StartGrids);
   

            while (StartGrids.Count > 0)
            {
                DrawableSection ds = StartGrids.Pop();
                drawPlaceholderOnStartgrid(ref ds);

                ds.Draw();
            }

        }

        public static void DrawTrack (Track t)
        {
            
            // build a drawable section list
            foreach ( Section s in t.Sections)
            {
                var drawable = new DrawableSection();
                drawable.section =s ;
                drawable.Trans.startPosition = cursor_start_position;

                string[] text;
                PickSectionGraphic(s, out text);
                drawable.asciiArt = text;

                // what will be the next cursor start position ?
                switch (MyHeadings.current)
                {
                    case Heading.South:
                        cursor_start_position.y += 5;
                        break;
                    case Heading.Nord:
                        cursor_start_position.y -= 5;
                        break;
                    case Heading.East:
                        cursor_start_position.x += 5;
                        break;
                    case Heading.West:
                        cursor_start_position.x -= 5;
                        break;
                }

                drawable.Draw();
                sectionsToRender.Add(drawable);
                
            }

            placePlaceholderParticipantsOnTrack();

            DrawParticipantsOnTrack(sectionsToRender);

            resetRenderer();

            
        }

        private static void resetRenderer()
        {
            if( sectionsToRender != null)
            {
                sectionsToRender.Clear();

            }
            else
            {
                sectionsToRender = new List<DrawableSection>();
            }
            cursor_start_position = (0, 3);
            MyHeadings.current = Heading.Nord;
            MyHeadings.last = MyHeadings.current;
            ParticipantPlaceholder = 1;
        }

        private static void GetStartGrids(out Stack<DrawableSection> StartGrids)
        {
            StartGrids = new Stack<DrawableSection>();
            foreach (var drawable in sectionsToRender)
            {
                if (drawable.section.SectionType == SectionTypes.StartGrid)
                    StartGrids.Push(drawable);
            }
        }

        private static void DrawParticipantsOnTrack(List<DrawableSection> sections)
        {
            foreach ( DrawableSection ds in sections)
            {
                SectionData sd = Data.CurrentRace.GetSectionData(ds.section);
                if (sd.Left != null)
                {
                    // Draw particpant on left (index 1)
                    char[] temp = ds.asciiArt[1].ToCharArray();
                    temp[3] = sd.Left.Name[0];
                    ds.asciiArt[1] = new string(temp);

                }
                if (sd.Right != null)
                {
                    // Draw particpant on right (index 3) 

                    char[] temp = ds.asciiArt[3].ToCharArray();
                    temp[3] = sd.Right.Name[0];
                    ds.asciiArt[3] = new string(temp);
                }
                else
                {
                    string[] graphic ;
                    PickSectionGraphic(ds.section, out graphic);
                    ds.asciiArt = graphic;
                }
            
                ds.Draw();
            }
        }

        private static void PickSectionGraphic (Section s, out string[] graphic )
        {
            SectionTypes type = s.SectionType;
            switch (type)
            {

                case SectionTypes.LeftCorner:
                    ChangeHeading(ref type);
                    graphic = (string[]) orientateGraphic(() => {
                        if( MyHeadings.last == Heading.East && MyHeadings.current == Heading.South)
                        {
                            return Graphics._rightCornerHorizontal;
                        }
                        if ( MyHeadings.last == Heading.West && MyHeadings.current == Heading.South)
                        {
                            return Graphics._leftCornerHorizontal;
                        }
                        if ( MyHeadings.last == Heading.East && MyHeadings.current == Heading.Nord)
                        {
                            return Graphics._leftCornerHorizontal;
                        }
                        if ( MyHeadings.last == Heading.West && MyHeadings.current == Heading.Nord)
                        {
                            return Graphics._rightCornerHorizontal;
                        }
                        return  Graphics._leftCornerHorizontal ;
                    }).Clone();
                    break;
                    
                case SectionTypes.RightCorner:
                    ChangeHeading(ref type);
                    graphic = (string[]) orientateGraphic(() => {
                        return Graphics._rightCornerHorizontal;
                    }).Clone();
                    break;

                case SectionTypes.StartGrid:
                  graphic =  (string[]) orientateGraphic(() =>
                    {
                        string[] choosen = new string[1];

                        if (MyHeadings.current.Equals(Heading.Nord) || MyHeadings.current.Equals(Heading.South)) {
                            choosen= Graphics._startGridVertical;
                        }else
                        {
                            choosen = Graphics._startGridHorizontal;
                        }

                        return choosen;
                    }).Clone();
                    break;

                case SectionTypes.Straight:
                  graphic =  (string[]) orientateGraphic(() => {
                        return MyHeadings.current.Equals(Heading.Nord) ||
                               MyHeadings.current.Equals(Heading.South) ? Graphics._straightVertical : Graphics._straightHorizontal;
                    }).Clone();
                    break;

                case SectionTypes.Finish:
                  graphic = (string[]) orientateGraphic(() => {
                        return MyHeadings.current.Equals(Heading.Nord) ||
                               MyHeadings.current.Equals(Heading.South) ? Graphics._finishVertical : Graphics._finishHorizontal;
                    }).Clone();
                    break;

                default:
                    // Err : unkown Section type 
                    graphic = new string[] { "?" };
                    break;
            }
        }

        private static void ChangeHeading( ref SectionTypes sectionType)
        {
            if( sectionType == SectionTypes.LeftCorner)
            {
                MyHeadings.last = MyHeadings.current;
                MyHeadings.current = MyHeadings.current.Equals(Heading.West) ? Heading.Nord : MyHeadings.current + 1;
            } 

            if( sectionType == SectionTypes.RightCorner)
            {
                MyHeadings.last = MyHeadings.current;
                MyHeadings.current = MyHeadings.current.Equals(Heading.Nord)? Heading.West : MyHeadings.current - 1;
            }
        }

        private static string[] orientateGraphic(chooseArtHeading chooseArt)
        {
            if (MyHeadings.last == Heading.South  || MyHeadings.last == Heading.West) 
            {
                return reverse(chooseArt()) ;
            } else
            {
                return chooseArt();
            }

        }


        private static string[] reverse(string[] k)
        {
            var result = new string[k.Length];
            var stack = new Stack<string>();
            foreach ( string s in k)
            {
                stack.Push(s);
            }

            int c = 0;
            while ( stack.Count > 0)
            {
                result[c]= stack.Pop();
                c++;
            }

            if (MyHeadings.last == Heading.East)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (string) result[i].Reverse();
                }
            }

            return result;
        }

        // TODO/NOTE: We might want to move this to the DrawableSection class
        private static void  drawPlaceholderOnStartgrid(ref DrawableSection section)
        {
            string[] startGrid = section.asciiArt;
            // NOTE: add placeholder
            if (ParticipantPlaceholder > Data.Competition.particpants.Count)
            {
                char[] temp = startGrid[3].ToCharArray();
                temp[3] = '▓';
                startGrid[3] = new string(temp);
            }
            else
            {

                char[] temp = startGrid[3].ToCharArray();
                temp[3] = ParticipantPlaceholder.ToString()[0];
                startGrid[3] = new string(temp);
                ParticipantPlaceholder++;

            }

            if (ParticipantPlaceholder > Data.Competition.particpants.Count)
            {
                char[] temp = startGrid[1].ToCharArray();
                temp[3] = '▓';
                startGrid[1] = new string(temp);

             
            }
            else
            {
                char[] temp = startGrid[1].ToCharArray();
                temp[3] = ParticipantPlaceholder.ToString()[0];
                startGrid[1] = new string(temp);
                ParticipantPlaceholder++;



            }

            section.asciiArt = startGrid;
        }

    }
}
