using Controller;
using Model;
using System;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
      
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.PlaceParticipants();
            
            Visualize.Initialize();
            Visualize.DrawTrack(Data.CurrentRace?.Track);
            Data.CurrentRace.Start();


            for(; ; )
            {
                Thread.Sleep(100); 
            }

        }
    }
}
