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
        private Map map;
        private ScrollViewer myScroller;
        private Canvas myCanvas;

        private aHex selectedHex;
        private aObject selectedObject;

        const int scroll_by = 1;

        static Point mouse_start = new Point(0, 0);
        private Boolean isDragging = false;


        public aHexMap(ScrollViewer scroller, Map map, Canvas canvas)
        {
            this.map = map;
            myScroller = scroller;
            myCanvas = canvas;
            _children = new VisualCollection(this);

            
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
        public void drawMap(Player p)
        {
            _children.Clear();
            drawBackground();
            var visible = map.visibleTiles(p);
            foreach (var tile in map.TileList)
            {
                int x_off, y_off;
                computeHexOffsets(tile.X, tile.Y, out x_off, out y_off);
                aHex tmpHex = null;
                switch (tile.Type)
                {
                    case TileType.Field:
                        tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                    ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, TryFindResource("field") as BitmapImage, tile);
                        break;
                    case TileType.Water:
                        tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                   ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, TryFindResource("water") as BitmapImage, tile);
                        break;
                    case TileType.Mountain:
                        tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                   ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, TryFindResource("mountain") as BitmapImage, tile);
                        break;
                    case TileType.Forest:
                        tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                   ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, TryFindResource("forest") as BitmapImage, tile);
                        break;
                    default:
                        tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                   ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, TryFindResource("field") as BitmapImage, tile);
                        break;
                }
                _children.Add(tmpHex);

                if (visible.Contains(tile))
                {
                    

                    int contents = 0;
                    foreach (GameObject go in tile.ContentList)
                    {
                        contents++;
                        BitmapImage aBackground = TryFindResource("castle") as BitmapImage;
                        var player = go.Owner;
                        if (go is Treasure)
                            aBackground = TryFindResource("coin_game") as BitmapImage;
                        if (go is Unit)
                        {
                            if (player.Neutral)
                                aBackground = TryFindResource("unit_yellow") as BitmapImage;
                            if (player.PlayerId.Equals(p.PlayerId))
                                aBackground = TryFindResource("unit_blue") as BitmapImage;
                            else
                                aBackground = TryFindResource("unit_red") as BitmapImage;
                        }
                        if (go is Building)
                        {
                            if (player.PlayerId.Equals(p.PlayerId))
                                aBackground = TryFindResource("castle_blue") as BitmapImage;
                            else
                                aBackground = TryFindResource("castle_red") as BitmapImage;
                        }
                        int xoffset = 0;
                        int yoffset = 0;

                        if (contents == 1)
                        {
                            xoffset = x_off + (ConstantValues.HEX_WIDTH / 10);
                            yoffset = y_off + (ConstantValues.HEX_WIDTH / 10);
                        }
                        if (contents == 2)
                        {
                            xoffset = x_off + (ConstantValues.HEX_WIDTH / 2);
                            yoffset = y_off + (ConstantValues.HEX_WIDTH / 10);
                        }
                        if (contents == 3)
                        {
                            xoffset = x_off + (ConstantValues.HEX_WIDTH / 4);
                            yoffset = y_off + (ConstantValues.HEX_WIDTH / 2);
                        }

                        aObject tempO = new aObject(xoffset, yoffset, ConstantValues.OBJECT_WIDTH, ConstantValues.OBJECT_HEIGHT, aBackground, tmpHex, go);


                        _children.Add(tempO);
                    }
                }
                else 
                {
                    tmpHex.Opacity = 0.5;
                }
            }

        }
  
        private void computeHexOffsets(int col, int row, out int x_off, out int y_off)
        {
            float fCol = (float)col;
            float fRow = (float)row;
            float fWidth = (float)ConstantValues.HEX_WIDTH;
            float fHeight = (float)ConstantValues.HEX_HEIGHT;
            float fGap = (float)ConstantValues.HEX_GAP;

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

            aObject obj = ht.VisualHit as aObject;
            if (obj != null) 
            {
                if (selectedObject != null && obj.Equals(selectedObject))
                {
                    updateObject(selectedObject);
                    selectedObject = null;
                }
                else
                {
                    if(selectedObject != null)
                    {
                        updateObject(selectedObject);
                        selectedObject = obj;
                        updateObject(selectedObject);
                    }
                    else
                    {
                        selectedObject = obj;
                        updateObject(selectedObject);
                    }
                }
                
                return HitTestResultBehavior.Stop;
            }

            // use casting to determine if click was on hex or map.
            aHex hex = ht.VisualHit as aHex;
            if (hex != null && selectedObject != null)
            {
                //updateHex(hex);
                selectedObject.Hex.Tile.ContentList.Remove(selectedObject.GameObject);
                hex.Tile.ContentList.Add(selectedObject.GameObject);
                drawMap(selectedObject.GameObject.Owner);
                selectedObject = null;
                selectedHex = null;
            } 
            return HitTestResultBehavior.Stop;
        }
        private void updateHex(aHex hex)
        {
            hex.Opacity = (hex.Opacity < 0.9f) ? 1.0f : 0.5f;
        }

        private void updateObject(aObject obj)
        {
            obj.Opacity = (obj.Opacity < 0.9f) ? 1.0f : 0.5f;
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
