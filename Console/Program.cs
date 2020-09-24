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
            Visualize.Initialize();
            Visualize.DrawTrack(Data.CurrentRace?.Track);
            for(; ; )
            {
                Thread.Sleep(100); 
            }

        }
    }
}
