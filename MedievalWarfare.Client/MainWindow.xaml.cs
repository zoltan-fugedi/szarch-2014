﻿using System;
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
using System.ComponentModel;


namespace MedievalWarfare.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServerMethodsCallback, INotifyPropertyChanged
    {
        
        private Proxy.ServerMethodsClient proxy;
        private aHexMap myMap;
        private Game game;
        private Player player;
        private String _message;

        public event PropertyChangedEventHandler PropertyChanged;
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message) 
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }




 

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            proxy = new ServerMethodsClient(new InstanceContext(this));
            player = new Player();
            game = new Game();
            Message = "Connect to the Server via the Main Menu";
            proxy.Open();

        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void ActionResult(bool result)
        {

            Dispatcher.BeginInvoke(new Action(() =>
            {
                //tbBox1.Text = igaz.ToString();
            }));

        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="mapInfo"></param>
        public void StartTurn(Common.Game mapInfo)
        {
            game = mapInfo;
            myMap = new aHexMap(mapScroller, game.Map, mapCanvas);
            mapCanvas.Children.Add(myMap);
            if (myMap != null)
            {
                myMap.drawMap(player);
               
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public void Update(Common.Utility.Command cmd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="winner"></param>
        public void EndGame(bool winner)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
             
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void menu_connect_Click(object sender, RoutedEventArgs e)
        {
            await proxy.JoinAsync(player);
            game = await proxy.GetGameStateAsync();

            myMap = new aHexMap(mapScroller, game.Map, mapCanvas);
            mapCanvas.Children.Add(myMap);
            if (myMap != null)
            {
                myMap.drawMap(player);

            }
            Message = "Connected To server";
        }

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}
