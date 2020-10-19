using Controller;
using Microsoft.VisualBasic.CompilerServices;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp
{
    public enum Heading
    {
        Nord,
        East,
        South,
        West

    }
    public static class Visualize
    {
        #region Graphics

        public static string[] _startGridHorizontal = new string[]
        {
                "═════", // 0
                "   ┋ ", // 1
                "   ┋ ", // 2 
                "   ┋ ", // 3
                "═════"  // 4
        };
        public static string[] _startGridVertical = new string[] {
                "║╍╍╍╍╍║",
                "║     ║",
                "║     ║",
                "║     ║"
            };

        public static string[] _finishVertical = new string[]
        {


                "║▬▬▬▬▬║",
                "║     ║",
                "║     ║",
                "║     ║"
        };
        public static string[] _finishHorizontal = new string[]
     {
                "════",
                "▐   ",
                "▐   ",
                "▐   ",
                "════"
     };

        public static string[] _straightHorizontal = new string[]
      {
                "═════",
                "     ",
                "  -  ",
                "     ",
                "═════"
      };
        public static string[] _straightVertical = new string[]
        {
                "║  |  ║",
                "║     ║",
                "║     ║",
                "║  |  ║"

         };

        // from nord
        public static string[] _TopRightCorner = new string[] {
                "══════╗",
                "      ║",
                "      ║",
                "      ║",
                "      ║"
        };
        
        public static string[] _TopLeftCorner = new string[]
        {
                "╔═══════",
                "║      ",
                "║      ",
                "║      ",
                "║      "
        };

        // from south 
        public static string[] _BottomRightCorner = new string[]
        {
            "       ║",
            "       ║",
            "       ║",
            "       ║",
            "═══════╝"
        };
        public static string[] _BottomLeftCorner = new string[]
        {
            "║       ",
            "║       ",
            "║       ",
            "║       ",
            "╚══════"
        };
        #endregion

      
        public static void Initialize()
        {
            Console.CursorVisible = false;
            Console.Title = "Race Simulator";
            Console.OutputEncoding = System.Text.Encoding.Default;

            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.NewRace += onNewRace;
          //  Console.SetCursorPosition(0, 40);
          //  Console.WriteLine($"Race has {Data.Competition.particpants.Count} participants");
          //  Console.WriteLine($"Racing on Track {Data.CurrentRace.Track.Name}");
        }

        private static void onNewRace(object sender, EventArgs e)
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.PlaceParticipants();
            Data.CurrentRace.Start();
        }

        public static void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            //Console.SetCursorPosition(0, 53);
            //Console.Write($"OnDriversChanged in Visualize! { DateTime.Now}");
            DrawTrack(args.track);
        }

        public static void DrawTrack(Track t)
        {
            List<SectionBuildingDetails> buildingDetails = new List<SectionBuildingDetails>();

            FillBuildingDetails(ref buildingDetails, t);
            ShiftPositions(buildingDetails);
            DrawBuildingDetails(ref buildingDetails);

            // Draw Players 

            foreach ( var sbd in buildingDetails)
            {
                SectionData sd = Data.CurrentRace.GetSectionData(sbd.section);
                if (sd.Left != null)
                {
                    if (sbd.direction != Heading.South || sbd.direction != Heading.Nord)
                    {
                        Console.SetCursorPosition((sbd.x * SectionBuildingDetails.SIZE_X) + 2, (sbd.y * SectionBuildingDetails.SIZE_Y) + 1);
                        if (sd.Left.Equipment.IsBroken)
                        {
                            Console.Write("✶");
                        }
                        else
                        {
                            Console.Write(sd.Left.Name[0]);
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition((sbd.x * SectionBuildingDetails.SIZE_X) + 1, (sbd.y * SectionBuildingDetails.SIZE_Y) + 2);
                        if (sd.Left.Equipment.IsBroken)
                        {
                            Console.Write("✶");
                        }
                        else
                        {
                            Console.Write(sd.Left.Name[0]);
                        }
                    }
                }

                if (sd.Right != null)
                {
                    if (sbd.direction != Heading.South || sbd.direction != Heading.Nord)
                    {
                        Console.SetCursorPosition((sbd.x * SectionBuildingDetails.SIZE_X) + 2, (sbd.y * SectionBuildingDetails.SIZE_Y) + 3);
                        if (sd.Right.Equipment.IsBroken)
                        {
                            Console.Write("✶");
                        }
                        else
                        {
                            Console.Write(sd.Right.Name[0]);
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition((sbd.x * SectionBuildingDetails.SIZE_X) + 3, (sbd.y * SectionBuildingDetails.SIZE_Y) + 2);
                        if (sd.Right.Equipment.IsBroken)
                        {
                            Console.Write("✶");
                        }
                        else
                        {
                            Console.Write(sd.Right.Name[0]);
                        }
                    }
                }
            }

        }
        private static void FillBuildingDetails(ref List<SectionBuildingDetails> buildingDetails, Track t)
        {
            int x = 0;
            int y = 0;
            Heading current = Heading.South;
            Heading last = current;

            foreach (Section sec in Data.CurrentRace.Track.Sections)
            {
                var a = new SectionBuildingDetails() { section = sec, x = x, y = y };
                // Determine its heading
                if (sec.SectionType == SectionTypes.LeftCorner)
                {
                    last = current;

                    if (current == Heading.West)
                    {
                        current = Heading.Nord;
                    }
                    else
                    {

                        current++;
                    }
                }
                if (sec.SectionType == SectionTypes.RightCorner)
                {
                    last = current;

                    if (current == Heading.Nord)
                    {
                        current = Heading.West;
                    }
                    else
                    {
                        current--;
                    }
                }

                a.direction = current;
                a.lastDirection = last;

                buildingDetails.Add(a);

                // Move position values based on the direction 
                switch (current)
                {
                    case (Heading.Nord):
                        y++;
                        break;
                    case (Heading.East):
                        x++;
                        break;
                    case (Heading.South):
                        y--;
                        break;
                    case (Heading.West):
                        x--;
                        break;
                };



            }

        }

        private static void ShiftPositions(List<SectionBuildingDetails> buildingDetails)
        {
            int offsetX = getLowestXValue(ref buildingDetails);
            int offsetY = getLowestYValue(ref buildingDetails);

            for (int i = 0; i < buildingDetails.Count; i++) {
                SectionBuildingDetails bd = buildingDetails[i];
                int real_x = buildingDetails[i].x + Math.Abs(offsetX);
                int real_y = buildingDetails[i].y + Math.Abs(offsetY);
                bd.x = real_x;
                bd.y = real_y;

                buildingDetails[i] = bd;
            }
        }

        private static int getLowestXValue(ref List<SectionBuildingDetails> buildingDetails)
        {
            int lowestValue = buildingDetails[0].x;
            foreach (SectionBuildingDetails bd in buildingDetails)
            {
                if (bd.x < lowestValue)
                    lowestValue = bd.x;
            }
            return lowestValue;
        }
        private static int getLowestYValue(ref List<SectionBuildingDetails> buildingDetails)
        {
            int lowestValue = buildingDetails[0].y;
            foreach (SectionBuildingDetails bd in buildingDetails)
            {
                if (bd.y < lowestValue)
                    lowestValue = bd.y;
            }
            return lowestValue;
        }

        private static void DrawBuildingDetails(ref List<SectionBuildingDetails> buildingDetails)
        {
            foreach (var bd in buildingDetails)
            {
                drawSection(bd);
            }
        }
         
        private static void drawSection( SectionBuildingDetails details)
        {
            SectionTypes sectionType = details.section.SectionType;
            (int x, int y) pos = (details.x * SectionBuildingDetails.SIZE_X, details.y * SectionBuildingDetails.SIZE_Y);
           if ( sectionType == SectionTypes.RightCorner || sectionType == SectionTypes.LeftCorner)
            {
                if (details.direction == Heading.East) {
                    if (details.lastDirection == Heading.Nord)
                    {
                        drawToConsole(_BottomLeftCorner, pos);
                    }

                    if (details.lastDirection == Heading.South)
                    {  
                        drawToConsole(_TopLeftCorner, pos);
                    }

                }
                
                if( details.direction == Heading.Nord)
                {
                    if (details.lastDirection == Heading.East)
                    {
                        drawToConsole(_TopRightCorner, pos);
                    }

                    if (details.lastDirection == Heading.West)
                    {
                        drawToConsole(_TopLeftCorner, pos);
                    }
                }

                if( details.direction == Heading.South)
                {
                    if (details.lastDirection == Heading.East)
                    {
                        drawToConsole(_BottomRightCorner, pos);
                    }

                    if (details.lastDirection == Heading.West)
                    {
                        drawToConsole(_BottomLeftCorner, pos);
                    }
                }

                if( details.direction == Heading.West)
                {
                    if (details.lastDirection == Heading.Nord)
                    {
                        drawToConsole(_BottomRightCorner, pos);
                    }

                    if (details.lastDirection == Heading.South)
                    {
                        drawToConsole(_TopRightCorner, pos);
                    }
                }
            }

            switch (sectionType)
            {
                case SectionTypes.StartGrid:
                    if ( details.direction == Heading.Nord || details.direction == Heading.South)
                    {
                        drawToConsole(_startGridVertical,pos);
                    }
                    else
                    {
                        drawToConsole(_startGridHorizontal,pos);
                    }
                    break;

                case SectionTypes.Straight:
                    if (details.direction == Heading.Nord || details.direction == Heading.South)
                    {
                        drawToConsole(_straightVertical,pos);
                    }
                    else
                    {
                        drawToConsole(_straightHorizontal,pos);
                    }

                    break;

                case SectionTypes.Finish:
                    if (details.direction == Heading.Nord || details.direction == Heading.South)
                    {
                        drawToConsole(_finishVertical,pos);
                    }
                    else
                    {
                        drawToConsole(_finishHorizontal,pos);
                    }

                    break;
            }

        }

        private static void drawToConsole(string[] graphic, (int x, int y) position, bool reversed = false)
        {
            // At this point I can assume the x and y given to me are correct.
            // Note: This might not work because the given y position is incorrect thus
            // not drawing it correctly
            Console.SetCursorPosition(position.x, position.y);
            if ( reversed)
            {
                int line = 0;
                for( int i = graphic.Length - 1 ; i > 0; i--)
                {
                    Console.SetCursorPosition(position.x, position.y + line);
                    Console.Write(graphic [i] );
                    line++;

                }
                return;
            }
            else {
                int line = 0;
                foreach (var s in graphic)
                {
                    Console.SetCursorPosition(position.x, position.y + line);
                    Console.Write(s);
                    line++;
                }
            }
            

        }

    }

    public struct SectionBuildingDetails
    {
        public const int SIZE_X = 5;
        public const int SIZE_Y = 5;
        public Section section;
        public int x, y;
        public Heading direction;
        public Heading lastDirection;
    }
}
