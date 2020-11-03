using Controller;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RaceStat raceStatsWindow;
        private CompStat compStatsWindow;

        public MainWindow()
        {
            InitializeComponent();
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.PlaceParticipants();

            // Set Datacontext 
            DataContext = new Context();

          
            Visualize.Initialize();
            Visualize.DrawTrack(Data.CurrentRace?.Track);
            Data.CurrentRace.Start();
            TrackImage.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                TrackImage.Source = null;
                    
                TrackImage.Source = Visualize.DrawTrack(Data.CurrentRace.Track);

            }));

   

        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Comp_Stat_Click(object sender, RoutedEventArgs e)
        {
            if(raceStatsWindow == null)
            {
                raceStatsWindow = new RaceStat();
                raceStatsWindow.Show();

            }
            raceStatsWindow.Activate();
        }

        private void Race_Stat_Click(object sender, RoutedEventArgs e)
        {
            if( compStatsWindow == null)
            {
                compStatsWindow = new CompStat();
                compStatsWindow.Show();

            }
            compStatsWindow.Activate();
        }
    }
}
