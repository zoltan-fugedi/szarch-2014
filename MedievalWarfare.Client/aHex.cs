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

namespace MedievalWarfare.Client
{
    class aHex : DrawingVisual
    {
        private int myLeft;
        private int myTop;
        private int myWidth;
        private int myHeight;
        private Brush myColor;
        List<Point> lines = new List<Point>();

        public aHex(int left, int top, int width, int height, Brush color)
        {
            myLeft = left;
            myTop = top;
            myWidth = width;
            myHeight = height;
            myColor = color;
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
