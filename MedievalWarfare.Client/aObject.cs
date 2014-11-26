using MedievalWarfare.Common.Utility;
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


namespace MedievalWarfare.Client
{
    class aObject : DrawingVisual
    {
        public aHex Hex { get; set; }
        public GameObject GameObject { get; set; }
        private int x_off;
        private int y_off;
        private int myWidth;
        private int myHeight;
        BitmapImage aBackground;
        

        public aObject(int left, int top, int width, int height,  BitmapImage bck, aHex parent, GameObject go )
        {
            x_off = left;
            y_off = top;
            myWidth = width;
            myHeight = height;
            aBackground = bck;
            Hex = parent;
            GameObject = go;
            displayObject();
        }
        private void displayObject()
        {
            using (DrawingContext dc = RenderOpen())
            {
                Rect aRec = new Rect(x_off , y_off , myWidth, myHeight);
                dc.DrawImage(aBackground, aRec);
            }
        }

    }
}
