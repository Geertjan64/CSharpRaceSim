using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public  readonly struct Graphics
    {
        #region graphics
        /* Horizontal graphics */
        public static readonly string[] _finishHorizontal = new string[]
        {
                "----",
                "#",
                "#",
                "#",
                "----"
        };
        public static readonly string[] _startGridHorizontal = new string[]
        {
                "----",
                "▓",
                "▓",
                "▓",
                "----"
        };
        public static readonly string[] _straightHorizontal = new string[]
        {
                "----",
                "",
                "-",
                "",
                "----"
        };

        public static readonly string[] _leftCornerHorizontal = new string[] {
                " /-----",
                "/      ",
                "|      ",
                "|      ",
                "|      "
            };
        public static readonly string[] _rightCornerHorizontal = new string[]
        {
                "-----\\ ",
                "      \\",
                "      |",
                "      |",
                "      |"
        };

        /* Vertical graphics*/
        public static readonly string[] _finishVertical = new string[]
        {


                "|###|",
                "|   |",
                "|   |",
                "|   |"
        };
        public static readonly string[] _startGridVertical = new string[] {
                "|〓〓〓|",
                "|   |",
                "|   |",
                "|   |"
            };
        public static readonly string[] _straightVertical = new string[]
        {
                "|  |  |",
                "|     |",
                "|     |",
                "|  |  |"

         };

        public static readonly string[] _leftCornerVertical = new string[]
        {
                "------\\",
                "      \\",
                "      |",
                "      |",
                "      |"
        };
        public static readonly string[] _rightCornerVertical = new string[]
        {
                "/------",
                "/      ",
                "|      ",
                "|      ",
                "|      "
        };
        #endregion
    }
}
