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
        private int myLeft;
        private int myTop;
        private int myWidth;
        private int myHeight;
        private Brush myColor;
        List<Point> lines = new List<Point>();

        public aHex(int X, int Y, int left, int top, int width, int height, Brush color, Tile tile)
        {
            myLeft = left;
            myTop = top;
            myWidth = width;
            myHeight = height;
            myColor = color;
            this.X = X;
            this.Y = Y;
            Tile = tile;
            buildHex();
            displayHex();
        }
        private void buildHex()
        {
            Point p = new Point(Math.Round(myWidth / 4.0) + myLeft, 0 + myTop);
            lines.Add(p);
            p = new Point(Math.Round((myWidth * 3.0) / 4.0) - 1 + myLeft, 0 + myTop);
            lines.Add(p);
            p = new Point(myWidth + myLeft - 1, Math.Round(myHeight / 2.0) + myTop - 1);
            lines.Add(p);
            p = new Point(Math.Round((myWidth * 3.0) / 4.0) + myLeft - 1, myHeight + myTop - 1);
            lines.Add(p);
            p = new Point(Math.Round(myWidth / 4.0) + myLeft, myHeight + myTop - 1);
            lines.Add(p);
            p = new Point(0 + myLeft, Math.Round(myHeight / 2.0) + myTop);
            lines.Add(p);

            
        }
        private void displayHex()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                dc.DrawGeometry(myColor, null, buildGeo());
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
