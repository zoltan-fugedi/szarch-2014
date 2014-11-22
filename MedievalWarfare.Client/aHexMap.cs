using MedievalWarfare.Common;
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

namespace MedievalWarfare.Client
{
    class aHexMap : FrameworkElement
    {
        private VisualCollection _children;
        //private BitmapImage aBackground;
        //private TextBlock aText;
        private Map map;
        private ScrollViewer myScroller;
        private Canvas myCanvas;

        const int scroll_by = 1;

        static Point mouse_start = new Point(0, 0);
        private Boolean isDragging = false;


        const int HEX_WIDTH = 60;
        const int HEX_HEIGHT = 60;
        const int HEX_GAP = 1;

        public aHexMap(ScrollViewer scroller, Map map, Canvas canvas)
        {
            //aText = tb;
            this.map = map;
            myScroller = scroller;
            myCanvas = canvas;
            _children = new VisualCollection(this);

            //aBackground = TryFindResource("grass") as BitmapImage;
            drawBackground();

            this.PreviewMouseUp += mouseClicked;
            this.PreviewMouseDown += mouseDown;
            this.PreviewMouseUp += mouseUp;
            this.PreviewMouseMove += mouseMoved;
        }

        private void drawBackground()
        {
            // Make Sure that height/width is set to external image dimensions.
            double width = myCanvas.ActualWidth;
            double height = myCanvas.ActualHeight;

            DrawingVisual mapBitmap = new DrawingVisual();
            using (DrawingContext dc = mapBitmap.RenderOpen())
            {
                Rect aRec = new Rect(0, 0, width, height);
                dc.DrawRectangle(new SolidColorBrush(Colors.Black), null, aRec);
            }
            _children.Add(mapBitmap);
        }
        public void drawHexes()
        {
            _children.Clear();
            drawBackground();
            foreach (var tile in map.TileList)
            {
                string hexName = string.Format("hex [{0},{1}]", tile.X, tile.Y);
                int x_off, y_off;
                computeHexOffsets(tile.X, tile.Y, out x_off, out y_off);
                aHex tmpHex = null;
                switch (tile.Type)
                {
                    case TileType.Field:
                        tmpHex = new aHex(x_off, y_off,
                    HEX_WIDTH, HEX_HEIGHT, Brushes.Green, hexName);
                        break;
                    case TileType.Water:
                        tmpHex = new aHex(x_off, y_off,
                   HEX_WIDTH, HEX_HEIGHT, Brushes.Blue, hexName);
                        break;
                    case TileType.Mountain:
                        tmpHex = new aHex(x_off, y_off,
                   HEX_WIDTH, HEX_HEIGHT, Brushes.Gray, hexName);
                        break;
                    default:
                        tmpHex = new aHex(x_off, y_off,
                   HEX_WIDTH, HEX_HEIGHT, Brushes.White, hexName);
                        break;
                }

                

                _children.Add(tmpHex);
            }
            foreach (var tile in map.TileList)
            {
                int x_off, y_off;
                computeHexOffsets(tile.X, tile.Y, out x_off, out y_off);
                foreach (var obj in tile.ContentList)
                {
                    BitmapImage aBackground = TryFindResource("castle") as BitmapImage;
                    double width = 30;
                    double height = 30;
                    DrawingVisual mapBitmap = new DrawingVisual();
                    using (DrawingContext dc = mapBitmap.RenderOpen())
                    {
                        Rect aRec = new Rect(x_off+HEX_WIDTH/4, y_off+HEX_HEIGHT/4, width, height);
                        dc.DrawImage(aBackground, aRec);
                    }
                    _children.Add(mapBitmap);
                }
            }
        }
        private void computeHexOffsets(int col, int row, out int x_off, out int y_off)
        {
            float fCol = (float)col;
            float fRow = (float)row;
            float fWidth = (float)HEX_WIDTH;
            float fHeight = (float)HEX_HEIGHT;
            float fGap = (float)HEX_GAP;

            x_off = (int)((fCol * (fWidth + fGap)) * 0.75f);
            float y_adjust = (fHeight * 0.5f) * (col % 2);
            y_off = (int)(fRow * (fHeight + fGap) + y_adjust);
        }
        private void mouseClicked(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);
            string Res = string.Format("mouse coords, relative to background = {0}", pt);
            VisualTreeHelper.HitTest(this, null,
                new HitTestResultCallback(someHexClicked),
                new PointHitTestParameters(pt));
        }
        void mouseDown(object sender, MouseEventArgs e)
        {
            mouse_start = e.GetPosition(myScroller);
            e.Handled = true;
        }
        void mouseMoved(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                scroll_map(e);
                mouse_start = e.GetPosition(myScroller);
            }
            e.Handled = true;
        }
        void mouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            e.Handled = true;
        }
        void scroll_map(MouseEventArgs e)
        {
            Point mouse_cur = e.GetPosition(myScroller);
            double x_diff = mouse_start.X - mouse_cur.X;
            double y_diff = mouse_start.Y - mouse_cur.Y;
            myScroller.ScrollToHorizontalOffset(myScroller.HorizontalOffset + x_diff);
            myScroller.ScrollToVerticalOffset(myScroller.VerticalOffset + y_diff);
        }
        public HitTestResultBehavior someHexClicked(HitTestResult ht)
        {
            // ignore clicking on a hex if map is being dragged.
            if (isDragging)
                return HitTestResultBehavior.Stop;
            // use casting to determine if click was on hex or map.
            aHex hex = ht.VisualHit as aHex;
            if (hex != null)
                updateMap(hex);
            return HitTestResultBehavior.Stop;
        }
        private void updateMap(aHex hex)
        {
            string hexData = string.Format("hex clicked = {0}", hex.myName);
            hex.Opacity = (hex.Opacity < 0.9f) ? 1.0f : 0.5f;
        }

        // Required overrides for FrameworkElement class.
        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return _children[index];
        }

    }
}
