using Model;
using System;

namespace ConsoleApp
{
    class DrawableSection
    {
        /*
         * This class is responsible for currectly displaying the section it 
         * represents on the console.
         */
        private Transform  _trans;
        public Section section { set; get;}
        public Transform Trans {
            set {
                _trans = value;
            }  
            get {
                return _trans;
            }
        } 
        public string[] asciiArt {  get;  set; }
        
        public DrawableSection()
        {
            Trans = new Transform();
        }
 
        // Draw should just be able to update 
        // those characters on the screen that belong to the section belonging 
        // to this object.
        public void Draw( ) {

            int line = 0;
            foreach ( string s in asciiArt)
            {
                // this calculation needs to get more complicated
                int cursorX = Trans.startPosition.x;
                int cursorY = Trans.startPosition.y + line;

                Console_Extension.DrawText((cursorX, cursorY), $"{s}");
                line++;
             
            }

        }

    }
}
