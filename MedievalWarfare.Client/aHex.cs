using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.Client
{
    class aHex : DrawingVisual
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tile Tile { get; set; }
        private int x_off;
        private int y_off;
        private int myWidth;
        private int myHeight;
        private Brush myColor;
        List<Point> lines = new List<Point>();

        private BitmapImage aBackground;


       /* public aHex(int X, int Y, int left, int top, int width, int height, Brush color, Tile tile)
        {
            x_off = left;
            y_off = top;
            myWidth = width;
            myHeight = height;
            myColor = color;
            this.X = X;
            this.Y = Y;
            Tile = tile;
            buildHex();
            displayHex();
        }*/

        public aHex(int X, int Y, int left, int top, int width, int height, BitmapImage image, Tile tile)
        {
            x_off = left;
            y_off = top;
            myWidth = width;
            myHeight = height;
            aBackground = image;
            this.X = X;
            this.Y = Y;
            Tile = tile;
            buildHex();
            displayHex();
        }
        private void buildHex()
        {
            Point p = new Point(Math.Round(myWidth / 4.0) + x_off, 0 + y_off);
            lines.Add(p);
            p = new Point(Math.Round((myWidth * 3.0) / 4.0) - 1 + x_off, 0 + y_off);
            lines.Add(p);
            p = new Point(myWidth + x_off - 1, Math.Round(myHeight / 2.0) + y_off - 1);
            lines.Add(p);
            p = new Point(Math.Round((myWidth * 3.0) / 4.0) + x_off - 1, myHeight + y_off - 1);
            lines.Add(p);
            p = new Point(Math.Round(myWidth / 4.0) + x_off, myHeight + y_off - 1);
            lines.Add(p);
            p = new Point(0 + x_off, Math.Round(myHeight / 2.0) + y_off);
            lines.Add(p);

            
        }
        private void displayHex()
        {
            using (DrawingContext dc = this.RenderOpen())
            {

                Rect aRec = new Rect(x_off, y_off, myWidth, myHeight);
                dc.DrawImage(aBackground, aRec);

                //dc.DrawGeometry(myColor, null, buildGeo());
            }
        }
        private Geometry buildGeo()
        {
            PathFigure pf = new PathFigure();
            pf.StartPoint = lines[0];
            for (int x = 1; x < lines.Count; x++)
                pf.Segments.Add(new LineSegment(lines[x], true));
            PathGeometry pg = new PathGeometry();
            pg.Figures.Add(pf);
            return pg;
        }
    }
}
