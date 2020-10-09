using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    class Console_Extension
    {

        /**
         * Contains  methods for the console 
         * to make it easier to draw !
         * */

        public static void DrawText ( (int Left, int Top) position , String text)
        {

            // Position is within bounds !
            if( position.Left > Console.BufferWidth  || position.Left < 0)
            {
                Console_Extension.DrawText((70,49), "\u001b[31m Left out of bounds\u001b[0m");
                return;
            }
            if (position.Top > Console.BufferHeight || position.Top < 0)
            {
                Console_Extension.DrawText((72, 50), "\u001b[31m Top out of bounds \u001b[0m");
                return;
            }

            Console.SetCursorPosition(position.Left, position.Top );
            Console.Write(text);
          //  Console.SetCursorPosition(0, 0);
        } 

    }
}
