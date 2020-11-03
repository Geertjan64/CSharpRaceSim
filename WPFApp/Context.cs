using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WPFApp
{
    class Context : INotifyPropertyChanged
    {

        public string TrackName {
            get {
                return Data.CurrentRace.Track.Name;
            }
            set {
                Data.CurrentRace.Track.Name = value;
            }
        }

        // Competition Information

        public List<Driver> drivers 
        {
            get {
                return new List<Driver>
                {
                    new Driver ("Sean", 200, TeamColors.Red),
                    new Driver("Michael", 120, TeamColors.Blue)
                };
            }
            set
            {

            }
        }


        // Race Information

        public int FastestLapTime 
        {
            get
            {
                return DateTime.Now.Hour;
            }
            set 
            { 
            }
        } 




        public event PropertyChangedEventHandler PropertyChanged;
        public Context()
        {
            Data.CurrentRace.DriversChanged += CurrentRace_DriversChanged;
        }

        private void CurrentRace_DriversChanged(object sender, Model.DriversChangedEventArgs e)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

}
