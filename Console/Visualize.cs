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
      


        private static (int x, int y) cursor_start_position;
        private static (Heading last, Heading current) MyHeadings;
        public delegate String[] chooseArtHeading();

        public static void Initialize()
        {
            cursor_start_position.x = 0;
            cursor_start_position.y = 0;
            MyHeadings.current = Heading.Nord;
            MyHeadings.last = MyHeadings.current;
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
                        return Graphics._leftCornerHorizontal;
                    });
                    break;

                case SectionTypes.RightCorner:
                    ChangeHeading(ref type);
                    DrawArt(() => {
                        return Graphics._rightCornerHorizontal;
                    });

                    break;

                case SectionTypes.StartGrid:
                    DrawArt(() =>
                    {
                        return MyHeadings.current.Equals(Heading.Nord) ||
                               MyHeadings.current.Equals(Heading.South) ? Graphics._startGridVertical : Graphics._startGridHorizontal;
                    });

                    break;

                case SectionTypes.Straight:
                    DrawArt(() => {
                        return MyHeadings.current.Equals(Heading.Nord) ||
                               MyHeadings.current.Equals(Heading.South) ? Graphics._straightVertical : Graphics._straightHorizontal;
                    });

                    break;

                case SectionTypes.Finish:

                    DrawArt(() => {
                        return MyHeadings.current.Equals(Heading.Nord) ||
                               MyHeadings.current.Equals(Heading.South) ? Graphics._finishVertical : Graphics._finishHorizontal;
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
                MyHeadings.last = MyHeadings.current;
                MyHeadings.current = MyHeadings.current.Equals(Heading.West) ? Heading.Nord : MyHeadings.current + 1;
            } 

            if( sectionType == SectionTypes.RightCorner)
            {
                MyHeadings.last = MyHeadings.current;
                MyHeadings.current = MyHeadings.current.Equals(Heading.Nord)? Heading.West : MyHeadings.current - 1;
            }
        }

        private static void DrawArt(chooseArtHeading chooseArt)
        {
            string[] Art = chooseArt();
            if (MyHeadings.last == Heading.South || MyHeadings.last == Heading.West ) 
            {
                // draw upside down
                int line = 0;
                Stack<string> stack = new Stack<string>();
                foreach (string s in Art){
                    stack.Push(s);
                }

                while ( stack.Count > 0)
                {
                    Console.SetCursorPosition(cursor_start_position.x, cursor_start_position.y + line);
                    if ( MyHeadings.last == Heading.East) {
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
                    Console.SetCursorPosition(cursor_start_position.x, cursor_start_position.y + line);
                    Console.Write(s);
                    line++;
                }

            }

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

       

            Console.SetCursorPosition( cursor_start_position.x , cursor_start_position.y );
        }

        private  enum Heading {
            Nord,
            East,
            South,
            West

        }


    }
}
