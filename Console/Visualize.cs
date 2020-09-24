using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp
{

    public static class Visualize
    {
        #region graphics
            /* Horizontal graphics */
            private static readonly string[] _finishHorizontal = new string[] 
            { 
                "----",
                "#",
                "#",
                "#",
                "----" 
            };
            private static readonly string[] _startGridHorizontal = new string[] 
            {
                "----",
                "▓",
                "▓",
                "▓", 
                "----"
            };
            private static readonly string[] _straightHorizontal = new string[] 
            { 
                "----",
                "",
                "-",
                "",
                "----" 
            };

            private static readonly string[] _leftCornerHorizontal = new string[] {
                " /-----",
                "/      ",
                "|      ",
                "|      ",
                "|      " 
            };
            private static readonly string[] _rightCornerHorizontal = new string[] 
            {
                "-----\\ ",
                "      \\",
                "      |",
                "      |",
                "      |"
            };

            /* Vertical graphics*/
            private static readonly string[] _finishVertical = new string[] 
            { 


                "|###|",
                "|   |",
                "|   |",
                "|   |" 
            };
            private static readonly string[] _startGridVertical = new string[] {
                "|〓〓〓|",
                "|   |",
                "|   |",
                "|   |"
            };
            private static readonly string[] _straightVertical= new string[]
            { 
                "|  |  |",
                "|     |",
                "|     |",
                "|  |  |"

             };

            private static readonly string[] _leftCornerVertical = new string[] 
            {
                "------\\",
                "      \\",
                "      |",
                "      |",
                "      |"
            };
            private static readonly string[] _rightCornerVertical = new string[] 
            {
                "/------",
                "/      ",
                "|      ",
                "|      ",
                "|      "
            };
        #endregion

        private static int cursor_start_x;
        private static int cursor_start_y;
        private static Heading current_heading;
        private static Heading last_heading;
        public delegate String[] chooseArtHeading();

        public static void Initialize()
        {
            cursor_start_x = 0;
            cursor_start_y = 0;
            current_heading = Heading.Nord;
            last_heading = current_heading;
        }
        public static void DrawTrack (Track t)
        {
            foreach (Section s in t.Sections)
            {
                DrawSection(s);
            }
        }
      
        private static void DrawSection (Section s)
        {
            SectionTypes type = s.SectionType;
            switch (type)
            {

                case SectionTypes.LeftCorner:
                    ChangeHeading(ref type);
                    DrawArt(() => {
                        if( last_heading == Heading.East && current_heading == Heading.South)
                        {
                            return _rightCornerHorizontal;
                        }
                        if ( last_heading == Heading.West && current_heading == Heading.South)
                        {
                            return _leftCornerHorizontal;
                        }
                        if ( last_heading == Heading.East && current_heading == Heading.Nord)
                        {
                            return _leftCornerHorizontal;
                        }
                        if ( last_heading == Heading.West && current_heading == Heading.Nord)
                        {
                            return _rightCornerHorizontal;
                        }
                        return _leftCornerHorizontal;
                    });
                    break;

                case SectionTypes.RightCorner:
                    ChangeHeading(ref type);
                    DrawArt(() => {
                        return _rightCornerHorizontal;
                    });

                    break;

                case SectionTypes.StartGrid:
                    DrawArt(() =>
                    {
                        return current_heading.Equals(Heading.Nord) ||
                               current_heading.Equals(Heading.South) ? _startGridVertical : _startGridHorizontal;
                    });

                    break;

                case SectionTypes.Straight:
                    DrawArt(() => {
                        return current_heading.Equals(Heading.Nord) ||
                               current_heading.Equals(Heading.South) ? _straightVertical : _straightHorizontal;
                    });

                    break;

                case SectionTypes.Finish:

                    DrawArt(() => {
                        return current_heading.Equals(Heading.Nord) ||
                               current_heading.Equals(Heading.South) ? _finishVertical : _finishHorizontal;
                    });
                    break;

                default:
                    // Err : unkown Section type 
                    break;
            }
        }


        /**
         * Watchout! this function has a side effect.
         * Effects the current heading .
         * */ 
        private static void ChangeHeading( ref SectionTypes sectionType)
        {
            if( sectionType == SectionTypes.LeftCorner)
            {
                last_heading = current_heading;
                current_heading = current_heading.Equals(Heading.West) ? Heading.Nord : current_heading + 1;
            } 

            if( sectionType == SectionTypes.RightCorner)
            {
                last_heading = current_heading;
                current_heading = current_heading.Equals(Heading.Nord)? Heading.West : current_heading - 1;
            }
        }

        private static void DrawArt(chooseArtHeading chooseArt)
        {
            string[] Art = chooseArt();
            if (last_heading == Heading.South || last_heading == Heading.West ) 
            {
                // draw upside down
                int line = 0;
                Stack<string> stack = new Stack<string>();
                foreach (string s in Art){
                    stack.Push(s);
                }

                while ( stack.Count > 0)
                {
                    Console.SetCursorPosition(cursor_start_x, cursor_start_y + line);
                    if ( last_heading == Heading.East) {
                        Console.Write(stack.Pop());
                    }else
                    {
                        Console.WriteLine(stack.Pop().Reverse().ToArray());
                    }
                    line++;
                }

            }
            else
            {
                int line = 0;
                foreach (string s in Art)
                {
                    Console.SetCursorPosition(cursor_start_x, cursor_start_y + line);
                    Console.Write(s);
                    line++;
                }

            }

            // what will be the next cursor start position ?
            switch (current_heading)
            {
                case Heading.South:
                    cursor_start_y += 5;
                    break;
                case Heading.Nord:
                    cursor_start_y -= 5;
                    break;
                case Heading.East:
                    cursor_start_x += 5;
                    break;
                case Heading.West:
                    cursor_start_x -= 5;
                    break;
            }

       

            Console.SetCursorPosition( cursor_start_x, cursor_start_y);
        }

        private  enum Heading {
            Nord,
            East,
            South,
            West

        }


    }
}
