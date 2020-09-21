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
            Console.WriteLine(Data.CurrentRace?.Track.Name);
            for(; ; )
            {
                Thread.Sleep(100); 
            }

        }
    }
}
