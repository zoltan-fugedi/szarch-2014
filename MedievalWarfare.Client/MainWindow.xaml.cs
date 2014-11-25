using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using MedievalWarfare.Client.Proxy;
using MedievalWarfare.Common;

namespace MedievalWarfare.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServerMethodsCallback
    {
        private Proxy.ServerMethodsClient proxy;
        private bool igaz = false;
        private aHexMap myMap;
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            proxy = new ServerMethodsClient(new InstanceContext(this));
            game = new Game();
            proxy.Open();
            
        }

        public void ActionResult(bool result)
        {
            igaz = result;

            Dispatcher.BeginInvoke(new Action(()=>
            {
                //tbBox1.Text = igaz.ToString();
            }));

        }

        public void StartTurn(Common.Game mapInfo)
        {
            game = mapInfo;
            myMap = new aHexMap(mapScroller, game.Map, mapCanvas);
            mapCanvas.Children.Add(myMap);
            if (myMap != null)
            {
                myMap.drawHexes();
                myMap.drawGameObjects();
                myMap.drawFOW();
            }
        }

        public void Update(Common.Utility.Command cmd)
        {
            throw new NotImplementedException();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            proxy.Join(new Player());
        }

        private async void menu_new_Click(object sender, RoutedEventArgs e)
        {
            await proxy.JoinAsync(new Player());
            //game.Map = await proxy.GetGameStateAsync();


        }
        private void drawHexes_Click(object sender, RoutedEventArgs e)
        {
            if (myMap != null) {
                myMap.drawHexes();
                myMap.drawGameObjects();
                myMap.drawFOW();
            }
               

        }
        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public void EndGame(bool winner)
        {
            throw new NotImplementedException();
        }
    }
}
