using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Configuration;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Controller;
using Model;
namespace WPFApp
{
    static class Visualize
    {
       // \Graphics\finish.bmp
        #region 
        private const string STRAIGHT = "Graphics\\straight.bmp";
        private const string CORNER = "Graphics\\corner.bmp";
        private const string FINISH = "Graphics\\finish.bmp";
        private const string STARTGRID = "Graphics\\start.bmp";
        private const string CAR = "Graphics\\car.bmp";
        #endregion

        private delegate int calc (int i);
        const int tileWidth = 40;
        const int tileHeight = 40;

        private static calc posX = (i) => i *  tileWidth;
        private static calc posY = (i) => i * tileHeight ;
        private static Bitmap bitmap;
        private static Graphics g;
        public static BitmapSource DrawTrack (Model.Track track)
        {

            #region WPF
            bitmap = new Bitmap(1000,1000);
           
      
            g = Graphics.FromImage(bitmap);


            g.FillRectangle(new SolidBrush(Color.Red), 0,0, 200, 200);

            #endregion


            List<SectionBuildingDetails> buildingDetails = new List<SectionBuildingDetails>();

            FillBuildingDetails(ref buildingDetails , track);
            ShiftPositions(buildingDetails);
            DrawBuildingDetails(ref buildingDetails);

            drawParticipants(ref buildingDetails);

            return ContentLoader.CreateBitmapSourceFromGdiBitmap(bitmap);

            
        }

        private static void DrawBuildingDetails(ref List<SectionBuildingDetails> buildingDetails)
        {
            foreach(var bd in buildingDetails)
            {
                DrawSection(bd);
            }
        }

        private static void DrawSection(SectionBuildingDetails details)
        {
            string resourcePath;

            // pick image
            switch (details.section.SectionType)
            {
                case (SectionTypes.Finish):
                    resourcePath = FINISH;
                    break;
                case (SectionTypes.RightCorner):
                case (SectionTypes.LeftCorner):
                    resourcePath = CORNER;
                    break;
                case (SectionTypes.StartGrid):
                    resourcePath = STARTGRID;
                    break;
                case (SectionTypes.Straight):
                    resourcePath = STRAIGHT;
                    break;
                
                default:
                    resourcePath = "";
                    break;
            }
            int x = posX(details.x);
            int y = posY(details.y);
            g.DrawImage(ContentLoader.GetBitmapFromCache(resourcePath),x, y, tileWidth, tileHeight);
        }
        struct SectionBuildingDetails
        {
            public Model.Section section;
            public int x, y ;
            public Heading direction, lastDirection;
        }
        enum Heading
        {
            North,
            South,
            East,
            West
        }

        private static void ShiftPositions ( List<SectionBuildingDetails> buildingDetails)
        {
            int offsetX = getLowestXValue(ref buildingDetails);
            int offsetY = getLowestYValue(ref buildingDetails);

            for(int i = 0; i < buildingDetails.Count; i++)
            {
                SectionBuildingDetails bd = buildingDetails[i];
                int real_x = buildingDetails[i].x + Math.Abs(offsetX);
                int real_y = buildingDetails[i].y + Math.Abs(offsetY);

                bd.x = real_x;
                bd.y = real_y;

                buildingDetails[i] = bd;
            }
        }

        private static int getLowestXValue (ref List<SectionBuildingDetails> buildingDetails)
        {
            int lowestValue = buildingDetails[0].x;
            foreach ( var bd in buildingDetails)
            {
                if (bd.x < lowestValue)
                    lowestValue = bd.x;
            }
            return lowestValue;
        }
        private static int getLowestYValue(ref List<SectionBuildingDetails> buildingDetails)
        {
            int lowestValue = buildingDetails[0].y;
            foreach (var bd in buildingDetails)
            {
                if (bd.y < lowestValue)
                    lowestValue = bd.y;
            }
            return lowestValue;
        }
        private static void FillBuildingDetails(ref List<SectionBuildingDetails> buildingDetails, Model.Track t)
        {
            int x = 0;
            int y = 0;

            Heading current = Heading.South;
            Heading last = current;

            foreach( Model.Section sec in Data.CurrentRace.Track.Sections)
            {
                var a = new SectionBuildingDetails() { section = sec, x = x, y = y };
                // Determine its heading 
                if (sec.SectionType == SectionTypes.LeftCorner)
                {
                    last = current;

                    if (current == Heading.West)
                    {
                        current = Heading.North;
                    }
                    else
                    {

                        current++;
                    }
                }
                if (sec.SectionType == SectionTypes.RightCorner)
                {
                    last = current;

                    if (current == Heading.North)
                    {
                        current = Heading.West;
                    }
                    else
                    {
                        current--;
                    }
                }

                a.direction = current;
                a.lastDirection = last;

                buildingDetails.Add(a);

                // move postion values based on the direction 
                switch (current)
                {
                    case(Heading.North):
                        y++;
                        break;
                    case (Heading.East):
                        x++;
                        break;
                    case (Heading.South):
                        y--;
                        break;
                    case (Heading.West):
                        x--;
                        break;

                }

            }
        }

        private static void drawParticipants(ref List<SectionBuildingDetails> buildingDetails)
        {
             foreach ( var sbd in buildingDetails)
            {
                SectionData sd = Data.CurrentRace.GetSectionData(sbd.section);

                if(sd.Left != null)
                {
                    g.DrawImage(ContentLoader.GetBitmapFromCache(CAR) , posX(sbd.x) - 14, posY(sbd.y) - 15);
                }

                if (sd.Right != null)
                {
                    g.DrawImage(ContentLoader.GetBitmapFromCache(CAR), posX(sbd.x) - 14, posY(sbd.y) - 15);
                }
            }
        }


        internal static void Initialize()
        {
            Data.CurrentRace.DriversChanged += CurrentRace_DriversChanged;
         
            Data.NewRace += Data_NewRace;
        }

        private static void Data_NewRace(object sender, EventArgs e)
        {
            Data.CurrentRace.DriversChanged += CurrentRace_DriversChanged;
            Data.CurrentRace.PlaceParticipants();
            Data.CurrentRace.Start();
        }

        private static void CurrentRace_DriversChanged(object sender, DriversChangedEventArgs e)
        {

            DrawTrack(e.track);
        }
    }
}
