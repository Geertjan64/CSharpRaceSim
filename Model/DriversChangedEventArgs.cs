using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {
        public DriversChangedEventArgs(Track track )
        {
            this.track = track;
        }
        public Track track { get; private set; }
    }
}
