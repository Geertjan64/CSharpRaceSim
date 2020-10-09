using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class Graphics
    {
        #region graphics
     
        public static  string[] _startGridHorizontal = new string[]
        {
                "---▓", // 0
                "   ▓", // 1
                "   ▓", // 2 
                "   ▓", // 3
                "---▓"  // 4
        };
        public static string[] _startGridVertical = new string[] {
                "〓〓〓〓〓",
                "|   |",
                "|   |",
                "|   |"
            };
      
        public static string[] _finishVertical = new string[]
        {


                "|###|",
                "|   |",
                "|   |",
                "|   |"
        };
        public static string[] _finishHorizontal = new string[]
     {
                "----",
                "#   ",
                "#   ",
                "#   ",
                "----"
     };

        public static string[] _straightHorizontal = new string[]
      {
                "----",
                "    ",
                "  - ",
                "    ",
                "----"
      };
        public static string[] _straightVertical = new string[]
        {
                "|  |  |",
                "|     |",
                "|     |",
                "|  |  |"

         };

        public static string[] _leftCornerHorizontal = new string[] {
                " /-----",
                "/      ",
                "|      ",
                "|      ",
                "|      "
        };
        public static string[] _leftCornerVertical = new string[]
        {
                "------\\",
                "      \\",
                "      |",
                "      |",
                "      |"
        };

        public static string[] _rightCornerHorizontal = new string[]
        {
                "-----\\ ",
                "      \\",
                "      |",
                "      |",
                "      |"
        };
        public static string[] _rightCornerVertical = new string[]
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
