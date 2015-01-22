using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedievalWarfare.Client
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MedievalWarfare.Client"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MedievalWarfare.Client;assembly=MedievalWarfare.Client"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:HexMapCanvas/>
    ///
    /// </summary>

    public class HexMapCanvas : FrameworkElement
    {
        static HexMapCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexMapCanvas), new FrameworkPropertyMetadata(typeof(HexMapCanvas)));
            
        }

        public ClientLogic Logic
        {
            get { return base.GetValue(LogicProperty) as ClientLogic; }
            set { base.SetValue(LogicProperty, value); }
        }
        public static readonly DependencyProperty LogicProperty =
          DependencyProperty.Register("Logic", typeof(ClientLogic), typeof(HexMapCanvas));


        private VisualCollection _children;
        private List<aHex> rangeIndicator;
        const int scroll_by = 1;
        static Point mouse_start = new Point(0, 0);
        private ScrollViewer scroll;
        private Boolean isDragging = false;


        

        public void Init()
        {
            _children = new VisualCollection(this);
            rangeIndicator = new List<aHex>();
            drawBackground();
            this.PreviewMouseUp += mouseClicked;
            this.PreviewMouseDown += mouseDown;
            this.PreviewMouseUp += mouseUp;
            this.PreviewMouseMove += mouseMoved;
            ScrollContentPresenter presenter = this.VisualParent as ScrollContentPresenter;
            scroll = presenter.ScrollOwner;
        }

        #region Visualization

        private void drawBackground()
        {
            // Make Sure that height/width is set to external image dimensions.
            double width = this.ActualWidth;
            double height = this.ActualHeight;

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
            var map = Logic.Game.Map;
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
                            {
                                aBackground = TryFindResource("unit_yellow") as BitmapImage;
                            }
                            else
                            {
                                if (player.PlayerId.Equals(p.PlayerId))
                                    aBackground = TryFindResource("unit_blue") as BitmapImage;
                                else
                                    aBackground = TryFindResource("unit_red") as BitmapImage;
                            }
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

        public void drawRangeIndicator(GameObject obj)
        {
            var map = Logic.Game.Map;
            var tiles = map.GetTilesInRange(obj.Tile, ((Unit)obj).Movement);
            foreach (var tile in tiles)
            {
                if (tile.traversable)
                {
                    int x_off, y_off;
                    computeHexOffsets(tile.X, tile.Y, out x_off, out y_off);
                    var tmpHex = new aHex(tile.X, tile.Y, x_off, y_off,
                        ConstantValues.HEX_WIDTH, ConstantValues.HEX_HEIGHT, Brushes.LightGreen, tile);
                    tmpHex.Opacity = 0.5;
                    _children.Add(tmpHex);
                    rangeIndicator.Add(tmpHex);
                }

            }
        }

        public void removeRangeIndicator()
        {
            foreach (var item in rangeIndicator)
            {
                _children.Remove(item);
            }
            rangeIndicator.Clear();
        }

        #endregion


        #region Selection and Scroll
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
            
            mouse_start = e.GetPosition(scroll);
            e.Handled = true;
        }
        void mouseMoved(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                scroll_map(e);
                mouse_start = e.GetPosition(scroll);
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
            
            Point mouse_cur = e.GetPosition(scroll);
            double x_diff = mouse_start.X - mouse_cur.X;
            double y_diff = mouse_start.Y - mouse_cur.Y;
            


            scroll.ScrollToHorizontalOffset(scroll.ContentHorizontalOffset + x_diff);
            scroll.ScrollToVerticalOffset(scroll.VerticalOffset + y_diff);
        }


        public HitTestResultBehavior someHexClicked(HitTestResult ht)
        {
            // ignore clicking on a hex if map is being dragged.
            if (isDragging)
                return HitTestResultBehavior.Stop;

            aObject obj = ht.VisualHit as aObject;
            if (obj != null)
            {
                Logic.ManageObjectSelection(obj);
                return HitTestResultBehavior.Stop;
            }

            // use casting to determine if click was on hex or map.
            aHex hex = ht.VisualHit as aHex;
            if (hex != null)
            {
                Logic.ManageTileSelection(hex);
            }
            return HitTestResultBehavior.Stop;
        }
        #endregion

        public void updateHex(aHex hex)
        {
            hex.Opacity = (hex.Opacity < 0.9f) ? 1.0f : 0.5f;
        }

        public void updateObject(aObject obj)
        {
            obj.Opacity = (obj.Opacity < 0.9f) ? 1.0f : 0.5f;

        }
        public void Clear()
        {
            _children.Clear();
        }

        // Required overrides for FrameworkElement class.
        protected override int VisualChildrenCount
        {
            get 
            {
                if (_children == null)
                {
                    _children = new VisualCollection(this);
                }
                
                return _children.Count; 
            }
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
